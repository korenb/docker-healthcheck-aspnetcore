
# Docker hub https://hub.docker.com/_/microsoft-dotnet-core

FROM microsoft/dotnet:2.2-aspnetcore-runtime as runtime-env
WORKDIR /app
EXPOSE 80
HEALTHCHECK --interval=2s --timeout=30s --start-period=5s --retries=3 \
    CMD curl --fail http://localhost/health || exit 1

FROM microsoft/dotnet:2.2-sdk as build-env
ARG DOTNET_BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet publish -c $DOTNET_BUILD_CONFIGURATION -o /app -v m

FROM runtime-env AS final
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT [ "dotnet", "AspDockerSample.dll" ]