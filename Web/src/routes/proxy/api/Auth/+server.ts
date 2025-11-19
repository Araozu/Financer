import { json, type RequestHandler } from '@sveltejs/kit';

export const POST: RequestHandler = async ({ request, cookies }) => {
	try {
		// Forward the request to the actual API endpoint
		const body = await request.text();

		const response = await fetch('https://example.com/api/Auth', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: body
		});

		if (!response.ok) {
			return json(
				{ error: 'Authentication failed' },
				{ status: response.status }
			);
		}

		// Parse the JSON response
		const data = await response.json();

		// Extract tokens
		const { access_token, refresh_token } = data;

		if (!access_token || !refresh_token) {
			return json(
				{ error: 'Invalid response from authentication server' },
				{ status: 500 }
			);
		}

		// Set cookies with the tokens
		// HTTPOnly and Secure flags for security
		cookies.set('access_token', access_token, {
			httpOnly: true,
			secure: true,
			sameSite: 'strict',
			path: '/',
			maxAge: 60 * 60 * 24 * 7 // 7 days
		});

		cookies.set('refresh_token', refresh_token, {
			httpOnly: true,
			secure: true,
			sameSite: 'strict',
			path: '/',
			maxAge: 60 * 60 * 24 * 30 // 30 days
		});

		// Return success response with tokens
		return json({
			success: true,
			access_token,
			refresh_token
		});

	} catch (error) {
		console.error('Proxy error:', error);
		return json(
			{ error: 'Internal server error' },
			{ status: 500 }
		);
	}
};
