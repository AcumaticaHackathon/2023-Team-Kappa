using System;
using PX.Data;

namespace HackathonKappa2023
{
  public class PipedreamSetupMaint : PXGraph<PipedreamSetupMaint>
  {

    public PXSave<KPSetup> Save;
    public PXCancel<KPSetup> Cancel;


    public PXSelect<KPSetup> SetupRec;

  }
}