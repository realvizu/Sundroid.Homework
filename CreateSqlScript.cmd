@echo off
pushd Persistence
dotnet ef migrations script --idempotent -o migration.sql
popd
