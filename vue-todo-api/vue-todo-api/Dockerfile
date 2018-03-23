FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY vue-todo-api.sln ./
COPY vue-todo-api/vue-todo-api.csproj vue-todo-api/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/vue-todo-api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "vue-todo-api.dll"]
