FROM mcr.microsoft.com/playwright/dotnet:v1.51.0-noble AS build

WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet build -c Release

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/playwright/dotnet:v1.51.0-noble AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright
RUN mkdir -p /ms-playwright && chown -R pwuser:pwuser /ms-playwright

USER pwuser

EXPOSE 5134

CMD ["dotnet", "WebScrapping.dll"]