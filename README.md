# CoderBash.Net.Vies

[![.NET](https://github.com/CoderBash/CoderBash.Net.Vies/actions/workflows/main.yml/badge.svg)](https://github.com/CoderBash/CoderBash.Net.Vies/actions/workflows/main.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=coderbash-net-vies&metric=alert_status&token=01c5786f7f875cc2c668c0f599a45b10580ae1af)](https://sonarcloud.io/summary/new_code?id=coderbash-net-vies)

Integrate the [VIES Service](https://ec.europa.eu/taxation_customs/vies/#/vat-validation) API into your .NET project!

The CoderBash VIES .NET Library targets the .NET 6 and .NET 7 frameworks.

## Installation with Nuget
To install the library via NuGet:
* Search for `CoderBash.Net.Vies` in the NuGet Library, or
* Type `Install-Package CoderBash.Net.Vies` in the NuGet Package Manager, or
* Install using the dotnet CLI with `dotnet add package CoderBash.Net.Vies`

## Usage
### 1. Validating VAT numbers.
To validate VAT numbers you can use the `ViesClient` class:

```C#
var client = new ViesClient();

var vatResult = await client.ValidateVatNumberAsync(EUCountryCodes.BE, "someVatNumber");

if (vatResult.isValid) {
  Console.WriteLine("Valid VAT number:");
  Console.WriteLine(" - Company = " + vatResult.Name);
  Console.WriteLine(" - Address = " + vatResult.Address);
} else {
  Console.WriteLine("Invalid VAT number.");
}
```

### 2. Validating multiple VAT numbers specify Timeout for HttpClient.
To validate VAT numbers you can use the `ViesClient` class:

```C#
var client = new ViesClient(new TimeSpan(0,1,0));

var vatResult = await client.ValidateVatNumberAsync(EUCountryCodes.BE, "someVatNumber");

if (vatResult.isValid) {
  Console.WriteLine("Valid VAT number:");
  Console.WriteLine(" - Company = " + vatResult.Name);
  Console.WriteLine(" - Address = " + vatResult.Address);
} else {
  Console.WriteLine("Invalid VAT number.");
}
```

### 3. Validating multiple VAT numbers.
To validate VAT numbers you can use the `ViesClient` class:

```C#

var vatNumbers = new List<string>(){"DE1234567","BE12334567","fr23486234"};
var client = new ViesClient();

var vatResult = await client.ValidateMultipleVatNumbersAsync(vatNumbers);

foreach(var item in vatResult)
{
  if (vatResult.isValid) {
    Console.WriteLine("Valid VAT number:");
    Console.WriteLine(" - Company = " + vatResult.Name);
    Console.WriteLine(" - Address = " + vatResult.Address);
  } else {
    Console.WriteLine("Invalid VAT number.");
  }
}

```


### 3. Get VAT Rate information
With the VatClient you can retrieve specific VAT rate information for a country. 

```C#
var client = new VatClient();

var rates = client.GetRatesForCountry("BE");
```
