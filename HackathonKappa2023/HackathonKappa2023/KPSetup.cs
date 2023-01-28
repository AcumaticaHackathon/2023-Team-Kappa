using System;
using PX.Data;

namespace HackathonKappa2023
{
  [Serializable]
  [PXCacheName("KPSetup")]
  public class KPSetup : IBqlTable
  {
    #region TestField
    [PXDBString(50, IsUnicode = true, InputMask = "")]
    [PXUIField(DisplayName = "Test Field")]
    public virtual string TestField { get; set; }
    public abstract class testField : PX.Data.BQL.BqlString.Field<testField> { }
    #endregion

    #region Tstamp
    [PXDBTimestamp()]
    [PXUIField(DisplayName = "Tstamp")]
    public virtual byte[] Tstamp { get; set; }
    public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
    #endregion
  }
}