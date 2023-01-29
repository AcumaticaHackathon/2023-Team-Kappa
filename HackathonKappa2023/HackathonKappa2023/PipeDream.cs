
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;


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
using PX.Data.BQL.Fluent;
using PX.Data.BQL;

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
					string fullName = notification.contactreply;
					string[] inteliname = fullName.Split(' ');

					var Q = new SelectFrom<Contact>.Where<Use<Contact.firstName>.AsString.IsEqual<@P.AsString>.
						And<Use<Contact.lastName>.AsString.IsEqual<@P.AsString>.
						And<Use<Contact.phone1>.AsString.IsEqual<@P.AsString>>>>.
						View(graph);

					CRLead lead;
					Contact c = Q.Select(inteliname[0], inteliname[1], notification.phonenumber).TopFirst;
					if (c != null)
					{
						graph.Lead.Current = graph.Lead.Search<CRLead.contactID>(c.ContactID);
						lead = graph.Lead.Current;
						graph.AddressCurrent.Select();
						lead.Description = "Updated from Text Campaing";
					}
					else
					{
						lead = graph.Lead.Insert(new CRLead());
						graph.Lead.Current = lead;
						graph.AddressCurrent.Current.CountryID = "US";
						graph.AddressCurrent.UpdateCurrent();
						lead.Description = "Created from Text Campaing";
						lead.FirstName = inteliname[0];
						lead.LastName = inteliname[1];
						lead.Phone1 = notification.phonenumber;
						lead.Phone1Type = "C";
					}

					PXNoteAttribute.SetNote(graph.Caches[typeof(CRLead)], lead, notification.cgptmessage);

					graph.Lead.Update(lead);
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