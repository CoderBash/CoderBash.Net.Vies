﻿using System;
using CoderBash.Net.Vies.Enums;
using CoderBash.Net.Vies.Exceptions;
using CoderBash.Net.Vies.Models;
using CoderBash.Net.Vies.Models.Internals;
using Newtonsoft.Json;

namespace CoderBash.Net.Vies.Clients
{
	/// <summary>
	/// Client for requesting a VAT number validation from the VIES service.
	/// </summary>
	public sealed class ViesClient : IDisposable
	{
		private readonly HttpClient _client;

		/// <summary>
		/// 
		/// </summary>
		public ViesClient()
		{
			_client = SetupClient();
		}

        /// <summary>
        /// Validate a VAT number for a specific country.
        /// </summary>
        /// <param name="countryCode">ISO 2 Code of the country. See <see cref="EUCountryCodes"/> for available options.</param>
        /// <param name="vatNumber">The VAT number to validate</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="VatValidationResponse"/> object.</returns>
        public async Task<VatValidationResponse> ValidateVatNumberAsync(string countryCode, string vatNumber, CancellationToken cancellationToken = default)
		{
			var country = (EUCountryCodes)Enum.Parse(typeof(EUCountryCodes), countryCode);

			return await ValidateVatNumberAsync(country, vatNumber, cancellationToken);
		}

        /// <summary>
        /// Validate a VAT number for a specific country.
        /// </summary>
        /// <param name="country">ISO 2 Code of the country. See <see cref="EUCountryCodes"/> for available options.</param>
        /// <param name="vatNumber">The VAT number to validate.</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="VatValidationResponse"/> object.</returns>
        /// <exception cref="ViesRequestException"></exception>
        /// <exception cref="ViesModelException"></exception>
        public async Task<VatValidationResponse> ValidateVatNumberAsync(EUCountryCodes country, string vatNumber, CancellationToken cancellationToken = default)
		{
			vatNumber = vatNumber.Replace(" ", "")
				.Replace(".", "")
				.Replace(country.ToString(), "")
				.Trim();

			var response = await _client.GetAsync($"{country}/vat/{vatNumber}", cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				throw new ViesRequestException($"An error occured fetching VAT information from VIES: {response.ReasonPhrase}");
			}

			var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

			if (responseContent == null)
			{
				throw new ViesRequestException("Unable to read response content from VAT information request.");
			}

			var viesResponse = JsonConvert.DeserializeObject<ViesResponse>(responseContent);

			if (viesResponse == null)
			{
				throw new ViesModelException("Could not deserialize expected VIES model from response content of the VAT information request.");
			}

			return new VatValidationResponse
			{
				Address = viesResponse.Address,
				Country = country,
				IsValid = viesResponse.IsValid,
				Name = viesResponse.Name,
				RequestDate = viesResponse.RequestDate,
				VatNumber = viesResponse.VatNumber
			};
		}

        #region IDisposable implementation
		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);

			_client.Dispose();
		}
        #endregion

        #region Setup
		private static HttpClient SetupClient()
		{
			var client = new HttpClient()
			{
				BaseAddress = new Uri("https://ec.europa.eu/taxation_customs/vies/rest-api/ms/")
			};

			return client;
		}
        #endregion
    }
}

