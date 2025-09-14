# Stage 1: Build the React application
FROM node:20-alpine as build

WORKDIR /app

# Copy package.json and package-lock.json first to leverage Docker cache
COPY package*.json ./
RUN npm install

# Copy the rest of the application source code
COPY . .

# Build the application for production
# This command runs `tsc && vite build` as defined in package.json
RUN npm run build

# Stage 2: Serve the application using Nginx
FROM nginx:1.25-alpine as production

# Nginx does not run as root by default in recent versions, which is good practice.
# We create a specific user/group for our app if needed, but for serving static files
# the default `nginx` user is sufficient.

# Copy the build output from the 'build' stage
COPY --from=build /app/build /usr/share/nginx/html

# Copy a custom Nginx configuration file. This is crucial for single-page applications (SPAs)
# as it directs all non-file requests to index.html to handle client-side routing.
# We assume a file `nginx.conf` exists in the project root.
# Example nginx.conf:
# server {
#   listen 80;
#   server_name localhost;
# 
#   root /usr/share/nginx/html;
#   index index.html;
# 
#   location / {
#     try_files $uri $uri/ /index.html;
#   }
# 
#   # Optional: Add headers for security
#   add_header X-Frame-Options "SAMEORIGIN" always;
#   add_header X-XSS-Protection "1; mode=block" always;
#   add_header X-Content-Type-Options "nosniff" always;
#   add_header Referrer-Policy "no-referrer-when-downgrade" always;
#   add_header Content-Security-Policy "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; font-src 'self' https://fonts.gstatic.com; object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'none';" always;
# }
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80 for the Nginx server
EXPOSE 80

# The default Nginx entrypoint will start the server.
CMD ["nginx", "-g", "daemon off;"]