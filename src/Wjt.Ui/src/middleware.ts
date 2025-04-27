import { NextRequest, NextResponse } from 'next/server'

export function middleware(request: NextRequest) {
    const nonce = Buffer.from(crypto.randomUUID()).toString('base64')
    const cspHeader = `
        default-src 'self';
        connect-src 'self' ${process.env.NEXT_PUBLIC_MOVIES_API_BASE_URL};
        script-src 'self' 'nonce-${nonce}' 'strict-dynamic';
        style-src 'self' ${process.env.NODE_ENV == 'production' ? `'nonce-${nonce}'` : `'unsafe-inline'`};
        img-src 'self' ${process.env.ALLOWED_IMAGE_SRC} blob: data:;
        font-src 'self';
        object-src 'none';
        base-uri 'self';
        form-action 'self';
        frame-ancestors 'none';
        ${process.env.NODE_ENV == 'production' ? 'upgrade-insecure-requests;' : ''}
    `
    // Replace newline characters and spaces
    const contentSecurityPolicyHeaderValue = cspHeader
        .replace(/\s{2,}/g, ' ')
        .trim()

    const requestHeaders = new Headers(request.headers)
    requestHeaders.set('x-nonce', nonce)

    requestHeaders.set(
        'Content-Security-Policy',
        contentSecurityPolicyHeaderValue
    )

    const response = NextResponse.next({
        request: {
            headers: requestHeaders,
        },
    })
    response.headers.set(
        'Content-Security-Policy',
        contentSecurityPolicyHeaderValue
    )

    return response
}