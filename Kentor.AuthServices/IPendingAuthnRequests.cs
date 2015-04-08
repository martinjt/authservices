using System.IdentityModel.Tokens;

namespace Kentor.AuthServices
{
	public interface IPendingAuthnRequests
	{
		void Add(Saml2Id id, StoredRequestState idp);
		bool TryRemove(Saml2Id id, out StoredRequestState idp);
	}
}