import type { paths } from "./api";
import createClient from "openapi-fetch";
import type { CreateMutationResult, CreateQueryResult } from "@tanstack/svelte-query";

export type { components } from "./api.d.ts";

/**
 * ASP.NET Core ProblemDetails type (RFC 7807)
 *
 * This is the standard error response format from our .NET backend.
 * See: https://datatracker.ietf.org/doc/html/rfc7807
 *
 * @property type - A URI reference identifying the problem type (defaults to "about:blank")
 * @property title - A short, human-readable summary of the problem
 * @property status - The HTTP status code
 * @property detail - A human-readable explanation specific to this occurrence
 * @property instance - A URI reference identifying the specific occurrence
 * @property [key: string] - Extension members (additional properties specific to the problem)
 */
export type ProblemDetails = {
	type: string;
	title: string;
	status: number;
	detail: string;
	instance: string | null;
	[key: string]: unknown; // Extension members
};

/**
 * Utility type to transform Tanstack Query results to have ProblemDetails as the error type.
 * Works with UseQueryResult, UseMutationResult, and UseInfiniteQueryResult.
 */
export type WithProblemDetails<T> = T extends CreateQueryResult<
	infer ResultType,
	unknown
>
	? CreateQueryResult<ResultType, ProblemDetails>
	: T extends CreateMutationResult<infer ResutType, unknown, infer FetchOptionsType>
		? CreateMutationResult<ResutType, ProblemDetails, FetchOptionsType>
		: T;

/**
 * Custom fetch implementation that includes credentials and handles errors.
 *
 * This wrapper transforms ASP.NET Core's ProblemDetails error responses (RFC 7807)
 * into consistently typed error objects that can be caught and handled by the frontend.
 *
 * The backend returns errors in three possible formats:
 * 1. JSON ProblemDetails object (application/json)
 * 2. Plain text error message (text/plain)
 * 3. Generic HTTP error (fallback)
 *
 * All errors are normalized to the ProblemDetails type for consistent handling.
 *
 * @param input - The fetch request URL or RequestInfo
 * @param init - Optional fetch request configuration
 * @returns The Response object if successful (2xx status)
 * @throws {ProblemDetails} Always throws ProblemDetails-shaped objects on error
 */
const enhancedFetch = async(
	input: RequestInfo | URL,
	init?: RequestInit,
): Promise<Response> =>
{
	if (process.env.NODE_ENV === "development")
	{
		await new Promise((resolve) => setTimeout(resolve, 500));
	}

	const response: Response = await fetch(input, {
		...init,
		credentials: "include",
	});

	if (!response.ok)
	{
		// Try to parse as C# ProblemDetails
		const contentType = response.headers.get("content-type");

		if (contentType?.includes("application/json"))
		{
			const problem = await response.json();

			// Basic validation that it's actually ProblemDetails
			if (problem && (problem.title || problem.detail || problem.status))
			{
				const problemDetails: ProblemDetails = {
					type: problem.type ?? "about:blank",
					title: problem.title ?? "Error del sistema",
					status: problem.status ?? response.status,
					detail: problem.detail ?? "Error del sistema",
					instance: problem.instance ?? null,
					...problem, // Include any extensions
				};
				throw problemDetails;
			}
		}
		else if (contentType?.includes("text/plain"))
		{
			const problemDetails: ProblemDetails = {
				type: "about:blank",
				title: "Error del sistema",
				status: response.status,
				detail: await response.text(),
				instance: null,
			};
			throw problemDetails;
		}

		// Fallback if response isn't proper ProblemDetails
		const problemDetails: ProblemDetails = {
			type: "about:blank",
			title: "Error HTTP",
			status: response.status,
			detail: response.statusText ?? `Error ${response.status}`,
			instance: null,
		};
		throw problemDetails;
	}

	return response;
};

/**
 * Client for connecting with the backend
 *
 * Uses openapi-fetch under the hood, wrapped with tanstack-query.
 * All query/mutation errors will be typed as ProblemDetails.
 */
export const api = createClient<paths>({
	fetch: enhancedFetch,
});

type FetchResult<A, B> = {
	data?: A;
	error?: B;
	response: Response;
};

export const sv =
	<Data, Error>(fn: () => Promise<FetchResult<Data, Error>>) => async(): Promise<Data> =>
	{
		const data = await fn();
		if (data.response.ok) return data.data!;
		else throw data.error;
	};