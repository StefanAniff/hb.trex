using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using NUnit.Framework;

namespace Trex.SmartClient.Test.Poc
{
    [TestFixture, Ignore("Just for pock'in and future refernce")]
    public class IsolationStoragePocTest
    {
        [Test]
        public void MethodName_SetupDescription_ExpectedResult()
        {
            TryStuff(() => IsolatedStorageFile.GetMachineStoreForAssembly(), "GetMachineStoreForAssembly");
            TryStuff(() => IsolatedStorageFile.GetMachineStoreForApplication(), "GetMachineStoreForApplication");
            TryStuff(() => IsolatedStorageFile.GetUserStoreForApplication(), "GetUserStoreForApplication");
            TryStuff(() => IsolatedStorageFile.GetUserStoreForDomain(), "GetUserStoreForDomain");
            TryStuff(() => IsolatedStorageFile.GetUserStoreForAssembly(), "GetUserStoreForAssembly");
            TryStuff(() => IsolatedStorageFile.GetUserStoreForSite(), "GetUserStoreForSite");
        }


        private void TryStuff(Action action, string methodName)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception exp)
            {
                Debug.Print("{0} -> FAILED! CAN ONLY RUN IN ClickOnce managed application", methodName);
                return;
            }

            Debug.Print("{0} -> SUCCESS! CAN RUN OUTSIDE CLICKONCE managed application", methodName);
        }
    }
}