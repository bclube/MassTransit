// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Tests
{
	using Configuration;
	using MassTransit.Serialization;
	using MassTransit.Transports;
	using NUnit.Framework;
	using Rhino.Mocks;

	[TestFixture]
	public class Configuration_Specs
	{
		[Test]
		public void Setting_a_specific_message_serializer_on_the_endpoint_should_work()
		{
			var objectBuilder = MockRepository.GenerateMock<IObjectBuilder>();

			var endpointFactory = EndpointFactoryConfigurator.New(x =>
				{
					x.SetObjectBuilder(objectBuilder);
					x.RegisterTransport<LoopbackEndpoint>();

					x.ConfigureEndpoint("loopback://localhost/mt_client", y => { y.SetSerializer<XmlMessageSerializer>(); });
				});

			objectBuilder.Stub(x => x.GetInstance<IEndpointFactory>()).Return(endpointFactory);
			objectBuilder.Expect(x => x.GetInstance(typeof (XmlMessageSerializer))).Return(new XmlMessageSerializer());

			IEndpoint endpoint = endpointFactory.GetEndpoint("loopback://localhost/mt_client");

			objectBuilder.VerifyAllExpectations();
		}
	}
}