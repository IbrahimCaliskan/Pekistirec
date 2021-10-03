using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine
{
    public class PekistirecEvents
    {
        public void Start()
        {
            DataLayer.Repositories.BaseRepositories.EtiketRepository.SaveCalled += DataLayer.Buffers.OzellikleriYenile;
            DataLayer.Repositories.BaseRepositories.GoogleRefreshTokenRepository.SaveCalled += DataLayer.Buffers.Yenile;
            DataLayer.Buffers.OzelliklerYenilendi += Pekistirec.Engine.PekistirecBuffers.jsBuffers.DokumanYukleBaslikArrayBufferYenile;
        }
    }
}