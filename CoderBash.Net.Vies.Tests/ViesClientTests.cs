using System;
using System.Runtime.CompilerServices;
using CoderBash.Net.Vies.Clients;
using CoderBash.Net.Vies.Enums;
using CoderBash.Net.Vies.Exceptions;

namespace CoderBash.Net.Vies.Tests
{
    [TestFixture(
        Author = "NicolasDemarbaix",
        Category = "CoderBash.Net.Vies",
        Description = "Vies Client tests",
        TestOf = typeof(ViesClient))]
    public class ViesClientTests
    {
        private const string TEST_VAT_NUMBER = "0729739314";
        private const string TEST_INVALID = "0000000000";
        private const string TEST_INVALID_FORMAT = "0729-739-314";
        private const string TEST_COMPANY_NAME = "DEMARBIT";

        [Test(
            Author = "NicolasDemarbaix",
            Description = "Fetch valid VAT information")]
        public void Test_Vies_FetchValidInfo()
        {
            using var client = new ViesClient();

            Assert.DoesNotThrowAsync(async () =>
            {
                var vatResult = await client.ValidateVatNumberAsync(Enums.EUCountryCodes.BE, TEST_VAT_NUMBER);

                Assert.Multiple(() =>
                {
                    Assert.That(vatResult, Is.Not.Null);
                    Assert.That(vatResult.IsValid, Is.True);
                    Assert.That(vatResult.Name, Is.EqualTo(TEST_COMPANY_NAME));
                });
            });
        }

        [Test(
            Author = "NicolasDemarbaix",
            Description = "Fetch valid VAT information (using country code)")]
        public void Test_Vies_FetchValidInfoUsingStringCode()
        {
            using var client = new ViesClient();

            Assert.DoesNotThrowAsync(async () =>
            {
                var vatResult = await client.ValidateVatNumberAsync("BE", TEST_VAT_NUMBER);

                Assert.Multiple(() =>
                {
                    Assert.That(vatResult, Is.Not.Null);
                    Assert.That(vatResult.IsValid, Is.True);
                    Assert.That(vatResult.Name, Is.EqualTo(TEST_COMPANY_NAME));
                    Assert.That(vatResult.Country, Is.EqualTo(EUCountryCodes.BE));
                });
            });
        }

        [Test(
            Author = "NicolasDemarbaix",
            Description = "Fetch invalid VAT information")]
        public void Test_Vies_FetchInvalidInfo()
        {
            using var client = new ViesClient();

            Assert.DoesNotThrowAsync(async () =>
            {
                var vatResult = await client.ValidateVatNumberAsync(EUCountryCodes.BE, TEST_INVALID);

                Assert.Multiple(() =>
                {
                    Assert.That(vatResult, Is.Not.Null);
                    Assert.That(vatResult.IsValid, Is.False);
                });
            });
        }

        [Test(
            Author = "NicolasDemarbaix",
            Description = "Incorrect VAT number format")]
        public void Test_Vies_InvalidVatNumberFormat()
        {
            using var client = new ViesClient();

            Assert.ThrowsAsync<ViesRequestException>(async () =>
            {
                await client.ValidateVatNumberAsync(EUCountryCodes.BE, TEST_INVALID_FORMAT);
            });
        }

        [Test(
            Author = "Robert Simon",
            Description = "HttpClient Timeout configure")]
        public void Test_Vies_TestHttpClientTimeout()
        {
            using var client = new ViesClient(new TimeSpan(0, 3, 0));
            Assert.DoesNotThrowAsync(async () =>
            {
                var vatResult = await client.ValidateVatNumberAsync(EUCountryCodes.BE, TEST_VAT_NUMBER);

                Assert.Multiple(() =>
                {
                    Assert.That(vatResult, Is.Not.Null);
                    Assert.That(vatResult.IsValid, Is.True);
                });
            });
        }


        [Test(Author = "Robert Simon", Description = "Check multiple vat number validation")]
        public void Test_Validate_Multiple_VATNumber()
        {
            var vatNumbers = new List<string>() { "DE327498938", "DE299733535", "DE260341043", "FR09238631849", "BE0364430967" };
            using var client = new ViesClient();
            Assert.DoesNotThrowAsync(async () =>
            {
                var validationResult = await client.ValidateMultipleVatNumbersAsync(vatNumbers);

                Assert.Multiple(() =>
                {
                    Assert.That(validationResult, Is.Not.Null);
                    Assert.That(validationResult, Is.All.Exist);
                    Assert.That(validationResult, Is.All.False);
                });
            });

        }
    }
}

