using MernisService;
using Project.BLL.DtoClasses;
using Project.BLL.Services.abstracts;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Project.BLL.Services.Concretes
{
    public class TCKimlikDogrulamaService : IIdentityValidationService
    {
        public async Task<bool> VerifyAsync(KimlikBilgisiDto dto)
        {
            KPSPublicSoapClient client = new KPSPublicSoapClient(KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap);

            string upperFirstName = dto.FirstName.ToUpper(new CultureInfo("tr-TR"));
            string upperLastName = dto.LastName.ToUpper(new CultureInfo("tr-TR"));
            long identityNumber = long.Parse(dto.IdentityNumber);

            TCKimlikNoDogrulaResponse response = await client.TCKimlikNoDogrulaAsync(
                identityNumber,
                upperFirstName,
                upperLastName,
                dto.BirthYear
            );

            return response.Body.TCKimlikNoDogrulaResult;
        }
    }
}
