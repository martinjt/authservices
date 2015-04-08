using System;
using System.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.IdentityModel.Metadata;
using Kentor.AuthServices.Internal;

namespace Kentor.AuthServices.Tests.Internal
{
    [TestClass]
    public class PendingAuthnRequestsTests
    {
        [TestMethod]
        public void PendingAuthnRequests_AddRemove()
        {
            var id = new Saml2Id();
	        var pendingAuthnRequests = new PendingAuthnRequests();
            var requestData = new StoredRequestState(new EntityId("testidp"), new Uri("http://localhost/Return.aspx"));
			pendingAuthnRequests.Add(id, requestData);
            StoredRequestState responseData;
			pendingAuthnRequests.TryRemove(id, out responseData).Should().BeTrue();
            responseData.Should().Be(requestData);
            responseData.Idp.Id.Should().Be("testidp");
            responseData.ReturnUrl.Should().Be("http://localhost/Return.aspx");
        }

        [TestMethod]
        public void PendingAuthnRequests_Add_ThrowsOnExisting()
        {
            var id = new Saml2Id();
			var pendingAuthnRequests = new PendingAuthnRequests();
			var requestData = new StoredRequestState(new EntityId("testidp"), new Uri("http://localhost/Return.aspx"));
			pendingAuthnRequests.Add(id, requestData);
			Action a = () => pendingAuthnRequests.Add(id, requestData);
            a.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void PendingAuthnRequests_Remove_FalseOnIdNeverIssued()
        {
            var id = new Saml2Id();
			var pendingAuthnRequests = new PendingAuthnRequests();
			StoredRequestState responseData;
			pendingAuthnRequests.TryRemove(id, out responseData).Should().BeFalse();
        }

        [TestMethod]
        public void PendingAuthnRequests_Remove_FalseOnRemovedTwice()
        {
            var id = new Saml2Id();
			var pendingAuthnRequests = new PendingAuthnRequests();
			var requestData = new StoredRequestState(new EntityId("testidp"), new Uri("http://localhost/Return.aspx"));
            StoredRequestState responseData;
			pendingAuthnRequests.Add(id, requestData);
			pendingAuthnRequests.TryRemove(id, out responseData).Should().BeTrue();
			pendingAuthnRequests.TryRemove(id, out responseData).Should().BeFalse();
        }

        [TestMethod]
        public void PendingAuthnRequest_TryRemove_NullGivesNull()
        {
            Saml2Id id = null;
			var pendingAuthnRequests = new PendingAuthnRequests();

            StoredRequestState state;

			pendingAuthnRequests.TryRemove(id, out state).Should().BeFalse();
            state.Should().BeNull();
        }
    }
}
