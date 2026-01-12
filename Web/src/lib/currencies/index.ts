import { createMutation, createQuery, useQueryClient } from "@tanstack/svelte-query";
import { api, sv, type ProblemDetails, type WithProblemDetails } from "$lib/../api";
import type { components } from "$lib/../api";

export type Currency = {
	id: string;
	code: string;
	name: string;
	symbol: string;
};

export type CreateCurrencyRequest = components["schemas"]["CreateCurrencyRequest"];
export type UpdateCurrencyRequest = components["schemas"]["UpdateCurrencyRequest"];

export const currencyKeys = {
	all: ["currencies"] as const,
	list: () => [...currencyKeys.all, "list"] as const,
	detail: (id: string) => [...currencyKeys.all, "detail", id] as const,
};

export function createCurrenciesQuery(): WithProblemDetails<ReturnType<typeof createQuery<Currency[]>>> {
	return createQuery({
		queryKey: currencyKeys.list(),
		queryFn: sv(() => api.GET("/api/Currencies")),
	}) as WithProblemDetails<ReturnType<typeof createQuery<Currency[]>>>;
}

export function createCurrencyQuery(id: string): WithProblemDetails<ReturnType<typeof createQuery<Currency>>> {
	return createQuery({
		queryKey: currencyKeys.detail(id),
		queryFn: sv(() => api.GET("/api/Currencies/{id}", { params: { path: { id } } })),
		enabled: !!id,
	}) as WithProblemDetails<ReturnType<typeof createQuery<Currency>>>;
}

export function createCurrencyMutation() {
	const queryClient = useQueryClient();

	return createMutation<{ id: string }, ProblemDetails, CreateCurrencyRequest>({
		mutationFn: async (data) => {
			const res = await api.POST("/api/Currencies", { body: data });
			if (!res.response.ok) throw res.error;
			return res.data as { id: string };
		},
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: currencyKeys.list() });
		},
	});
}

export function updateCurrencyMutation() {
	const queryClient = useQueryClient();

	return createMutation<void, ProblemDetails, { id: string; data: UpdateCurrencyRequest }>({
		mutationFn: async ({ id, data }) => {
			const res = await api.PUT("/api/Currencies/{id}", {
				params: { path: { id } },
				body: data,
			});
			if (!res.response.ok) throw res.error;
		},
		onSuccess: (_data, variables) => {
			queryClient.invalidateQueries({ queryKey: currencyKeys.list() });
			queryClient.invalidateQueries({ queryKey: currencyKeys.detail(variables.id) });
		},
	});
}

export function deleteCurrencyMutation() {
	const queryClient = useQueryClient();

	return createMutation<void, ProblemDetails, string>({
		mutationFn: async (id) => {
			const res = await api.DELETE("/api/Currencies/{id}", {
				params: { path: { id } },
			});
			if (!res.response.ok) throw res.error;
		},
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: currencyKeys.list() });
		},
	});
}
