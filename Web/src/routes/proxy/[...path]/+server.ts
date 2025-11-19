import { json, type RequestHandler, type RequestEvent } from "@sveltejs/kit";

const BACKEND_URL = "https://example.com";

async function handleRequest(event: RequestEvent<{ path: string }>) {
  const { request, cookies, params } = event;
  try {
    // Get the access token from cookies
    const accessToken = cookies.get("access_token");

    if (!accessToken) {
      return json(
        { error: "Unauthorized - No access token found" },
        { status: 401 },
      );
    }

    // Get the path from params
    const path = params.path || "";

    // Build the target URL
    const targetUrl = `${BACKEND_URL}/${path}`;

    // Get the request body if it exists
    let body = null;
    if (request.method !== "GET" && request.method !== "HEAD") {
      body = await request.text();
    }

    // Forward headers, but add/override Authorization
    const headers = new Headers();

    // Copy relevant headers from original request
    request.headers.forEach((value, key) => {
      // Skip host and other headers that shouldn't be forwarded
      if (!["host", "connection", "cookie"].includes(key.toLowerCase())) {
        headers.set(key, value);
      }
    });

    // Set Authorization header with Bearer token
    headers.set("Authorization", `Bearer ${accessToken}`);

    // Make the proxied request
    const response = await fetch(targetUrl, {
      method: request.method,
      headers: headers,
      body: body,
    });

    // Return the response directly without parsing/re-encoding
    return new Response(response.body, {
      status: response.status,
      headers: response.headers,
    });
  } catch (error) {
    console.error("Proxy error:", error);
    return json({ error: "Proxy request failed" }, { status: 500 });
  }
}

export const GET: RequestHandler = handleRequest;
export const POST: RequestHandler = handleRequest;
export const PUT: RequestHandler = handleRequest;
export const PATCH: RequestHandler = handleRequest;
export const DELETE: RequestHandler = handleRequest;
