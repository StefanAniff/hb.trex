<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CloudServices" generation="1" functional="0" release="0" Id="ec5b3643-1a81-43b2-a8fc-6fa5e3209a8d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CloudServicesGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Trex.Services:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CloudServices/CloudServicesGroup/LB:Trex.Services:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Trex.Services:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServices/CloudServicesGroup/MapTrex.Services:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Trex.ServicesInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudServices/CloudServicesGroup/MapTrex.ServicesInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Trex.Services:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CloudServices/CloudServicesGroup/Trex.Services/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapTrex.Services:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServices/CloudServicesGroup/Trex.Services/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapTrex.ServicesInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudServices/CloudServicesGroup/Trex.ServicesInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Trex.Services" generation="1" functional="0" release="0" software="C:\d60\Trex\T-rex\src\Trex.Administration.Service\CloudServices\csx\Test\roles\Trex.Services" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Trex.Services&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Trex.Services&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="Services.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudServices/CloudServicesGroup/Trex.ServicesInstances" />
            <sCSPolicyFaultDomainMoniker name="/CloudServices/CloudServicesGroup/Trex.ServicesFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="Trex.ServicesFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Trex.ServicesInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="b3ec0d43-ef10-4925-8ed8-0a3bd3431a75" ref="Microsoft.RedDog.Contract\ServiceContract\CloudServicesContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="66d1316e-ff72-48c5-9ec5-2f46b97f1f00" ref="Microsoft.RedDog.Contract\Interface\Trex.Services:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/CloudServices/CloudServicesGroup/Trex.Services:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>