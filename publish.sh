dotnet publish -c Release -r linux-x64 -p:PublishReadyToRun=true -p:TieredCompilation=false -p:PublishSingleFile=true --self-contained
dotnet publish -c Release -r win-x64   -p:PublishReadyToRun=true -p:TieredCompilation=false -p:PublishSingleFile=true --self-contained


rm -f "releases/linux-x64.tar.gz"
rm -f "releases/win-x64.zip"

tar -czf "releases/linux-x64.tar.gz" -C "bin/Release/net10.0/linux-x64/publish" .

(
    cd "bin/Release/net10.0/win-x64/publish" &&
    7z a -tzip "../../../../../releases/win-x64.zip"
)
