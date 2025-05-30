@echo off
setlocal

set SERVER=127.0.0.1
set USER=sa
set PASSWORD=Password.123
set DATABASE=datacollector
set SQLSCRIPT=Persistence/migration.sql

echo Dropping database [%DATABASE%] if it exists...
sqlcmd -S %SERVER% -U %USER% -P %PASSWORD% -Q "IF DB_ID('%DATABASE%') IS NOT NULL DROP DATABASE [%DATABASE%];"

echo Creating database [%DATABASE%]...
sqlcmd -S %SERVER% -U %USER% -P %PASSWORD% -Q "CREATE DATABASE [%DATABASE%];"

echo Running script [%SQLSCRIPT%] on [%DATABASE%]...
sqlcmd -S %SERVER% -U %USER% -P %PASSWORD% -d %DATABASE% -i %SQLSCRIPT%

echo Done.
endlocal
