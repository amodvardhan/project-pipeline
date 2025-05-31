import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: 'https://localhost:5247/api/:path*',
      },
    ];
  },
  // Disable HTTPS certificate verification for development
  // experimental: {
  //   serverComponentsExternalPackages: [],
  // },
};

 export default nextConfig;

// /** @type {import('next').NextConfig} */
// const nextConfig = {
  
// };

// module.exports = nextConfig;
