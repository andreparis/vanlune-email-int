dotnet publish C:\Projects\vanlune\vanlune-email-int\email.int.sln -c release -o C:\Projects\vanlune\vanlune-email-int\out
$compress = @{
  Path = "C:\Projects\vanlune\vanlune-email-int\out\*"
  CompressionLevel = "Fastest"
  DestinationPath = "C:\Projects\vanlune\vanlune-email-int\out\Email.Int.zip"
}
Compress-Archive @compress -Force

aws s3 cp C:\Projects\vanlune\vanlune-email-int\out\Email.Int.zip s3://vanlune-bin-dev
aws lambda update-function-code --function-name vanlune-email-int      --s3-bucket vanlune-bin-dev --s3-key Email.Int.zip
aws lambda update-function-code --function-name vanlune-email-int-dlq  --s3-bucket vanlune-bin-dev --s3-key Email.Int.zip