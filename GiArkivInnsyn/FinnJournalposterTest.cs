using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceReference;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

#if NET6_0_OR_GREATER
using System.Text.Json;
#endif

namespace GeoIntegrationClient.GiArkivInnsyn
{
	[TestClass]
	public class FinnJournalposterTest
	{
		[TestMethod]
		[DataRow("Eiendomsskatt Husebyveien")]
		public async Task HentJournalpost(string title)
		{
			var criterias = new List<Soekskriterie>();
			if (!string.IsNullOrEmpty(title))
				criterias.Add(GetAndCriteria("Journalpost.tittel", title));

			var response = await FinnJournalposterAsync(criterias.ToArray());
			Assert.IsNotNull(response, "Response should not be null");
			Assert.IsNotNull(response.@return, "Response.return should not be null");

#if NET6_0_OR_GREATER
			foreach(var item in response.@return)
			{
				Console.WriteLine(JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true}));
			}
#endif
		}


		static Soekskriterie GetAndCriteria(string field, string value)
		{
			return new Soekskriterie
			{
				@operator = SoekeOperatorEnum.EQ,
				Kriterie = new Soekefelt
				{
					feltnavn = field,
					feltverdi = value
				}
			};
		}

		private async Task<FinnJournalposterResponse> FinnJournalposterAsync(Soekskriterie[] sok, bool returnerMerknad = false, bool returnerTilleggsinformasjon = false, bool returnerKorrespondansepart = false, bool returnerAvskrivning = false)
		{
			using (var client = ArkivInnsynClientFactory.CreateArkivInnsynPortClient())
			{
				var kontekst = new ArkivKontekst { klientnavn = Configuration.ClientName };
				return await client.FinnJournalposterAsync(sok, returnerMerknad, returnerTilleggsinformasjon, returnerKorrespondansepart, returnerAvskrivning, kontekst);
			}
		}

	}

}