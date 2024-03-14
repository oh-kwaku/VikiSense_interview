
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["VikiSense_interview/VikiSense_interview.csproj", "VikiSense_interview/"]
RUN dotnet restore "VikiSense_interview/VikiSense_interview.csproj"
COPY . .
WORKDIR "/src/VikiSense_interview"
RUN dotnet build "VikiSense_interview.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VikiSense_interview.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VikiSense_interview.dll"]