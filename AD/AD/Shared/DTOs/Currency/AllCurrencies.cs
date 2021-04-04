using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Currency
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AllCurrencies
    {
        public results results { get; set; }
    }
    public class results
    {
        public ALL ALL { get; set; }
        public XCD XCD { get; set; }
        public EUR EUR { get; set; }
        public BBD BBD { get; set; }
        public BTN BTN { get; set; }
        public BND BND { get; set; }
        public XAF XAF { get; set; }
        public CUP CUP { get; set; }
        public USD USD { get; set; }
        public FKP FKP { get; set; }
        public GIP GIP { get; set; }
        public HUF HUF { get; set; }
        public IRR IRR { get; set; }
        public JMD JMD { get; set; }
        public AUD AUD { get; set; }
        public LAK LAK { get; set; }
        public LYD LYD { get; set; }
        public MKD MKD { get; set; }
        public XOF XOF { get; set; }
        public NZD NZD { get; set; }
        public OMR OMR { get; set; }
        public PGK PGK { get; set; }
        public RWF RWF { get; set; }
        public WST WST { get; set; }
        public RSD RSD { get; set; }
        public SEK SEK { get; set; }
        public TZS TZS { get; set; }
        public AMD AMD { get; set; }
        public BSD BSD { get; set; }
        public BAM BAM { get; set; }
        public CVE CVE { get; set; }
        public CNY CNY { get; set; }
        public CRC CRC { get; set; }
        public CZK CZK { get; set; }
        public ERN ERN { get; set; }
        public GEL GEL { get; set; }
        public HTG HTG { get; set; }
        public INR INR { get; set; }
        public JOD JOD { get; set; }
        public KRW KRW { get; set; }
        public LBP LBP { get; set; }
        public MWK MWK { get; set; }
        public MRO MRO { get; set; }
        public MZN MZN { get; set; }
        public ANG ANG { get; set; }
        public PEN PEN { get; set; }
        public QAR QAR { get; set; }
        public STD STD { get; set; }
        public SLL SLL { get; set; }
        public SOS SOS { get; set; }
        public SDG SDG { get; set; }
        public SYP SYP { get; set; }
        public AOA AOA { get; set; }
        public AWG AWG { get; set; }
        public BHD BHD { get; set; }
        public BZD BZD { get; set; }
        public BWP BWP { get; set; }
        public BIF BIF { get; set; }
        public KYD KYD { get; set; }
        public COP COP { get; set; }
        public DKK DKK { get; set; }
        public GTQ GTQ { get; set; }
        public HNL HNL { get; set; }
        public IDR IDR { get; set; }
        public ILS ILS { get; set; }
        public KZT KZT { get; set; }
        public KWD KWD { get; set; }
        public LSL LSL { get; set; }
        public MYR MYR { get; set; }
        public MUR MUR { get; set; }
        public MNT MNT { get; set; }
        public MMK MMK { get; set; }
        public NGN NGN { get; set; }
        public PAB PAB { get; set; }
        public PHP PHP { get; set; }
        public RON RON { get; set; }
        public SAR SAR { get; set; }
        public SGD SGD { get; set; }
        public ZAR ZAR { get; set; }
        public SRD SRD { get; set; }
        public TWD TWD { get; set; }
        public TOP TOP { get; set; }
        public VEF VEF { get; set; }
        public DZD DZD { get; set; }
        public ARS ARS { get; set; }
        public AZN AZN { get; set; }
        public BYR BYR { get; set; }
        public BOB BOB { get; set; }
        public BGN BGN { get; set; }
        public CAD CAD { get; set; }
        public CLP CLP { get; set; }
        public CDF CDF { get; set; }
        public DOP DOP { get; set; }
        public FJD FJD { get; set; }
        public GMD GMD { get; set; }
        public GYD GYD { get; set; }
        public ISK ISK { get; set; }
        public IQD IQD { get; set; }
        public JPY JPY { get; set; }
        public KPW KPW { get; set; }
        public LVL LVL { get; set; }
        public CHF CHF { get; set; }
        public MGA MGA { get; set; }
        public MDL MDL { get; set; }
        public MAD MAD { get; set; }
        public NPR NPR { get; set; }
        public NIO NIO { get; set; }
        public PKR PKR { get; set; }
        public PYG PYG { get; set; }
        public SHP SHP { get; set; }
        public SCR SCR { get; set; }
        public SBD SBD { get; set; }
        public LKR LKR { get; set; }
        public THB THB { get; set; }
        public TRY TRY { get; set; }
        public AED AED { get; set; }
        public VUV VUV { get; set; }
        public YER YER { get; set; }
        public AFN AFN { get; set; }
        public BDT BDT { get; set; }
        public BRL BRL { get; set; }
        public KHR KHR { get; set; }
        public KMF KMF { get; set; }
        public HRK HRK { get; set; }
        public DJF DJF { get; set; }
        public EGP EGP { get; set; }
        public ETB ETB { get; set; }
        public XPF XPF { get; set; }
        public GHS GHS { get; set; }
        public GNF GNF { get; set; }
        public HKD HKD { get; set; }
        public XDR XDR { get; set; }
        public KES KES { get; set; }
        public KGS KGS { get; set; }
        public LRD LRD { get; set; }
        public MOP MOP { get; set; }
        public MVR MVR { get; set; }
        public MXN MXN { get; set; }
        public NAD NAD { get; set; }
        public NOK NOK { get; set; }
        public PLN PLN { get; set; }
        public RUB RUB { get; set; }
        public SZL SZL { get; set; }
        public TJS TJS { get; set; }
        public TTD TTD { get; set; }
        public UGX UGX { get; set; }
        public UYU UYU { get; set; }
        public VND VND { get; set; }
        public TND TND { get; set; }
        public UAH UAH { get; set; }
        public UZS UZS { get; set; }
        public TMT TMT { get; set; }
        public GBP GBP { get; set; }
        public ZMW ZMW { get; set; }
        public BTC BTC { get; set; }
        public BYN BYN { get; set; }
        public BMD BMD { get; set; }
        public GGP GGP { get; set; }
        public CLF CLF { get; set; }
        public CUC CUC { get; set; }
        public IMP IMP { get; set; }
        public JEP JEP { get; set; }
        public SVC SVC { get; set; }
        public ZMK ZMK { get; set; }
        public XAG XAG { get; set; }
        public ZWL ZWL { get; set; }
    }
    public class ALL
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class XCD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class EUR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BBD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BTN
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class BND
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class XAF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class CUP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class USD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class FKP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class GIP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class HUF
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class IRR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class JMD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class AUD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class LAK
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class LYD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MKD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class XOF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class NZD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class OMR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class PGK
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class RWF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class WST
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class RSD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SEK
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class TZS
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class AMD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class BSD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BAM
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class CVE
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class CNY
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class CRC
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class CZK
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class ERN
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class GEL
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class HTG
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class INR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class JOD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class KRW
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class LBP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class MWK
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MRO
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MZN
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class ANG
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class PEN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class QAR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class STD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class SLL
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class SOS
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SDG
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class SYP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class AOA
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class AWG
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BHD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class BZD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BWP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BIF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class KYD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class COP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class DKK
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class GTQ
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class HNL
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class IDR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class ILS
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class KZT
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class KWD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class LSL
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MYR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class MUR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class MNT
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class MMK
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class NGN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class PAB
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class PHP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class RON
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SAR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SGD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class ZAR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SRD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class TWD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class TOP
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class VEF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class DZD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class ARS
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class AZN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BYR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BOB
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BGN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class CAD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class CLP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class CDF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class DOP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class FJD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class GMD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class GYD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class ISK
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class IQD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class JPY
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class KPW
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class LVL
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class CHF
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class MGA
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MDL
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MAD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class NPR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class NIO
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class PKR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class PYG
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SHP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SCR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SBD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class LKR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class THB
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class TRY
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class AED
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class VUV
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class YER
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class AFN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BDT
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class BRL
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class KHR
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class KMF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class HRK
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class DJF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class EGP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class ETB
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class XPF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class GHS
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class GNF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class HKD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class XDR
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class KES
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class KGS
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class LRD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class MOP
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MVR
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class MXN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class NAD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class NOK
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class PLN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class RUB
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class SZL
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class TJS
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class TTD
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class UGX
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class UYU
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class VND
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class TND
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class UAH
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class UZS
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class TMT
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class GBP
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class ZMW
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class BTC
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BYN
    {
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
        public string id { get; set; }
    }

    public class BMD
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class GGP
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class CLF
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class CUC
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class IMP
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class JEP
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class SVC
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class ZMK
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class XAG
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }

    public class ZWL
    {
        public string currencyName { get; set; }
        public string id { get; set; }
    }
}
