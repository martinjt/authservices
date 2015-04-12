﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentor.AuthServices.Internal
{
	internal class PendingAuthnRequests : IPendingAuthnRequests
	{
        private static readonly Dictionary<Saml2Id, StoredRequestState> pendingAuthnRequest = new Dictionary<Saml2Id, StoredRequestState>();

		public void Add(Saml2Id id, StoredRequestState idp)
        {
            lock (pendingAuthnRequest)
            {
                if (pendingAuthnRequest.ContainsKey(id))
                {
                    throw new InvalidOperationException("AuthnRequest id can't be reused.");
                }
                pendingAuthnRequest.Add(id, idp);
            }
        }

		public bool TryRemove(Saml2Id id, out StoredRequestState idp)
        {
            lock (pendingAuthnRequest)
            {
                if (id != null && pendingAuthnRequest.ContainsKey(id))
                {
                    idp = pendingAuthnRequest[id];
                    return pendingAuthnRequest.Remove(id);
                }
                idp = null;
                return false;
            }
        }
    }
}
