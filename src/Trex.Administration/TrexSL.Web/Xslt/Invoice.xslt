<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:ms="urn:schemas-microsoft-com:xslt"
                xmlns:ext="http://example.org/extension" extension-element-prefixes="ext">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/">

    <html>
      <head>
        <style>
          body {font-family:Calibri;font-size:11pt;height:27.9cm; }
          h2 {font-size:13pt;display:inline;}
          h3 {font-size:11pt;display:inline;}
          #canvas {width:17.5cm;height:25.7cm;}
          #invoiceHeader {margin-top:1.4cm;height:3.5cm;margin-bottom:2.2cm;}
          #pageNumber {margin-top:0.7cm;}
          #invoiceDataPanel {border-bottom:solid 2px black; height:2.2cm;margin-bottom:0.3cm;}
          #invoiceText {float:left;margin-top:1.35cm;}
          #invoiceData {float:right;width:6.3cm;}
          #invoiceContentPanel { height:4.7cm;}
          #totalPanel {float:right;width:6.3cm;border-top:solid 1px black;margin-top:1cm;height:3.5cm;}
          #footerPanel {clear:both;height:2cm;}
          .dataText {float:left;
          width:3cm;}
          .dataValue {width:3cm;float:right;text-align:right;}
          .descriptionColumn {float:left;}
          .priceColumn {float:right;width:80px; text-align:right; }
          .vatColumn {float:right;margin-right:20px;width:100px;text-align:right;}
          .invoiceLine{clear:both;height:0.6cm;}
        </style>
      </head>
      <body>
        <xsl:apply-templates select="@* | node()" />
      </body>

    </html>

  </xsl:template>

  <xsl:template match="Invoice">
    <div id="canvas">
      <div id="invoiceHeader">
        <div id="customerName">
          <h2>
            <xsl:value-of select="CustomerName" />
          </h2>
        </div>
        <div>
          <xsl:value-of select="Attention" />
        </div>
        <div>
          <xsl:value-of select="StreetAddress" />
        </div>
        <div>
          <xsl:value-of select="Address2" />
        </div>
        <div>
          <xsl:value-of select="ZipCode" />
          <xsl:text></xsl:text>
          <xsl:value-of select="City" />
        </div>
        <div>
          <xsl:value-of select="Country" />
        </div>
        <div id="pageNumber">
          Side 1 af 1
        </div>
      </div>
      <div id="invoiceDataPanel">
        <div id="invoiceText">
          <h2>
            <xsl:if test="TotalInclVAT&gt;0">
              Faktura (
              <xsl:value-of select="ext:ToShortDate(StartDate)" />
              -
              <xsl:value-of select="ext:ToShortDate(EndDate)" />
              )
            </xsl:if>
            <xsl:if test="TotalInclVAT&lt;0">
              Kreditnota (
              <xsl:value-of select="ext:ToShortDate(StartDate)" />
              -
              <xsl:value-of select="ext:ToShortDate(EndDate)" />
              )
            </xsl:if>

          </h2>
        </div>
        <div id="invoiceData">
          <div class="dataText">
            Fakturadato
          </div>
          <div class="dataValue">
            <xsl:value-of select="ext:ToShortDate(InvoiceDate)" />
          </div>
          <div class="dataText">
            Forfaldsdato
          </div>
          <div class="dataValue">
            <xsl:value-of select="ext:ToShortDate(DueDate)" />
          </div>

          <div class="dataText">
            Fakturanr.
          </div>
          <div class="dataValue">
            <xsl:value-of select="ID" />
          </div>
          <div class="dataText">
            Kundenr.
          </div>
          <div class="dataValue">
            <xsl:value-of select="CustomerNumber" />
          </div>

        </div>

      </div>
      <div id="invoiceContentPanel">
        <div class="descriptionColumn">
          <div style="float:left;">
            <h3>Ydelsesbeskrivelse</h3>
          </div>

          <div style="float:left;margin-left:10px;">
            <xsl:value-of select="Regarding" />
          </div>
          <p />
        </div>

        <div class="priceColumn">
          <h3>Beløb DKK</h3>
          <p />
        </div>
        <div class="vatColumn">
          <h3>Moms DKK</h3>
          <p></p>
        </div>
        <div id="invoiceLinePanel">
          <xsl:apply-templates select="ArrayOfInvoiceLine/InvoiceLine"></xsl:apply-templates>
        </div>
      </div>
      <div id="totalPanel">
        <div class="dataText">Momsgrundlag</div>
        <div class="dataValue">
          <xsl:value-of select="ext:ToFormattedNumber(TotalExclVAT)" />
        </div>

        <div class="dataText">Moms udgør</div>
        <div class="dataValue">
          <xsl:value-of select="ext:ToFormattedNumber(VATAmount)" />
        </div>
        <div class="dataText">
          <strong>Totalbeløb DKK</strong>
        </div>
        <div class="dataValue">
          <strong>
            <xsl:value-of select="ext:ToFormattedNumber(TotalInclVAT)" />
          </strong>
        </div>
      </div>
      <div id="footerPanel">
        Ovenstående beløb bedes indbetalt til
        <strong>Danske Bank</strong>
        regnr.
        <strong>3420</strong>
        kontonr.
        <strong>3420298668.</strong>
        Angiv venligst fakturanummer
        <strong>
          <xsl:value-of select="ID"></xsl:value-of>
        </strong>
        ved elektronisk bankoverførsel.
      </div>
    </div>

  </xsl:template>

  <xsl:template match="InvoiceLine">
    <div class="invoiceLine">
      <div class="descriptionColumn">
        <xsl:value-of select="ext:ToFormattedNumber(Units)" />
        <xsl:text></xsl:text>
        <xsl:value-of select="Unit" />
        á kr.
        <xsl:value-of select="ext:ToFormattedNumber(PricePrUnit)" />
        <xsl:text></xsl:text>
        <xsl:value-of select="Text" />
      </div>
      <div class="priceColumn">
        <xsl:value-of select="ext:ToFormattedNumber(Total)" />

      </div>
      <div class="vatColumn">
        <xsl:value-of select="ext:ToFormattedNumber(VatAmount)" />
        <xsl:text></xsl:text>
      </div>

    </div>
  </xsl:template>

  <ms:script language="C#" implements-prefix="ext">
    <![CDATA[
        public string ToShortDate(string dateTime)
        {
            if(string.IsNullOrEmpty(dateTime))
                return string.Empty;
            string date = dateTime.Substring(0, dateTime.IndexOf("T"));
            DateTime parsedDate = DateTime.Parse(date);
            return parsedDate.ToShortDateString();
        }
        
        public string ToFormattedNumber(double number)
        {
            return string.Format("{0:0,0.00}",number);
        }
  
  ]]>
  </ms:script>

</xsl:stylesheet>