
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using static PX.Data.BQL.BqlPlaceholder;
using static PX.EP.EPLoginType.allowedLoginType;

using static PX.Objects.CR.CaseSourcesAttribute;


using System.Net.Http;
using System.Net.Http.Headers;
using PX.Common;
using PX.Data;
using PX.Data.Webhooks;
using PX.Objects;
using PX.Objects.CR;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace HackathonKappa2023
{
	public class Notification
	{
		public string phonenumber { get; set; }
		public string contactreply { get; set; }
		public string cgptmessage { get; set; }
	}

	public class PipeFreamPushWebhookHandler : IWebhookHandler
	{
		public async Task<System.Web.Http.IHttpActionResult> ProcessRequestAsync(
		  HttpRequestMessage request, CancellationToken cancellationToken)
		{
			using (var scope = GetAdminScope())
			{
				try
				{
					// Request custom authorization header example
					var secret = string.Empty;

					if (request.Headers.TryGetValues("CustomAuthorization", out IEnumerable<string> headerValues))
					{
						secret = headerValues.FirstOrDefault();
					}

					// If secret does not match we reject the notification
					if (secret != "secretValue") return new StatusCodeResult(System.Net.HttpStatusCode.Unauthorized, request);
					string testValue = await request.Content.ReadAsStringAsync();
					// Deserialize JSON into our Notification class
					var notification = JsonConvert.DeserializeObject<Notification>(testValue);

					var graph = PXGraph.CreateInstance<LeadMaint>();
					CRLead lead = graph.Lead.Insert(new CRLead());
					graph.Lead.Current = lead;
					Address addr = graph.AddressCurrent.Insert(new Address() { CountryID = "US" });
					graph.AddressCurrent.Current= addr;	

					lead.Description = "Created from Text Campaing";


					string fullName = notification.contactreply;
					string[] inteliname = fullName.Split(' ');
					lead.FirstName = inteliname[0];
					lead.LastName = inteliname[1];
					lead.Phone1 = notification.phonenumber;
					lead.Phone1Type = "C";
					graph.Lead.Update(lead);
					graph.AddressCurrent.Update(addr);

					graph.Actions.PressSave();
				}
				catch (Exception ex)
				{
					var failed = new ExceptionResult(ex, false, new DefaultContentNegotiator(), request, new[] { new JsonMediaTypeFormatter() });

					return failed;
				}

			}

			return new OkResult(request); ;
		}

		private IDisposable GetAdminScope()
		{
			var userName = "admin";
			if (PXDatabase.Companies.Length > 0)
			{
				var company = PXAccess.GetCompanyName();
				if (string.IsNullOrEmpty(company))
				{
					company = PXDatabase.Companies[0];
				}
				userName = userName + "@" + company;
			}
			return new PXLoginScope(userName);
		}
	}
}