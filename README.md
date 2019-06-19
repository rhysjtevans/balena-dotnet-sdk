# Balena-Dotnet-SDK
Very simple static class that can be used to call against Balena API.



I quickly put this together to be able to change environment variables from the project I'm working on. As Balena doesn't have any C# Dotnet Core examples I thought I'd quickly share this, hopefully helping others get up and running quicker and not fall in to some of the issues I faced.

## Contributing back
Completely open to PR's and issues. I'm in no way a dotnet engineer or precious about the code so if I've implemented something poorly do please help fix it :).

## The goal
The goal of the project I'm working on is to be able to get/set environment variables. That's what I'll be aiming. 
If I have to hit other endpoints, I'll abstract methods out as best as possible making it easier to extend in the future.

If your contributions can align with above, that would be great!

## Getting Started Example
```
// Set some required defaults
String BALENA_API_URI = "https://api.balena-cloud.com/v4/"
String BALENA_API_TOKEN = "8d726f7479374610a5461638db705db9"
String BALENA_DEVICE_UUID = "1a404...";
String BALENA_APP_NAME = "balena_app_name";


BalenaCloudAPI balena = new BalenaCloudAPI(BALENA_API_URI, BALENA_API_TOKEN);

int deviceId = await balena.GetDeviceIdAsync(BALENA_DEVICE_UUID);
balena.SetDeviceVariable(deviceId, "SOMEKEY", "SOME SOME VARIABLE");

// or

int appId = await balena.GetApplicationIdAsync(BALENA_APP_NAME);
balena.SetApplicationVariable(appId, "SOME_APP_KEY", "SOME SOME VARIABLE");


```