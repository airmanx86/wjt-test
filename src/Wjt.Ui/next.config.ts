import type { NextConfig } from "next";

const nextConfig: NextConfig = {
    images: {
        remotePatterns: [new URL(`${process.env.ALLOWED_IMAGE_SRC}/**`)],
    }
};

export default nextConfig;
