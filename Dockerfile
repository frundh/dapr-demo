FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG APP_NAME
WORKDIR /src
COPY . ./
RUN dotnet publish "./src/${APP_NAME}" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ARG APP_NAME
WORKDIR /app
COPY --from=build /publish ./

# Create entrypoint, so we can pass arguments to the app, and use a arbitrary name for the binary
RUN echo '#!/bin/sh\n' \
         'exec dotnet "/app/'"${APP_NAME}.dll"'" "$@"\n' > ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]