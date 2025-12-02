using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Process_Info
{
    public static class ProcessInfoMapper
    {
        public static ProcessInfoMainDto ToMainDto(ProcessInfo entity)
        {
            if (entity == null) return null;
            return new ProcessInfoMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                ProcessInfo = new ProcessInfoDto
                {
                    Process_Volume_M3h = entity.Process_Volume_M3h,
                    Location = entity.Location,
                    ProcessVolumeMin = entity.ProcessVolumeMin,
                    Process_Acrmax = entity.Process_Acrmax,
                    ClothArea = entity.ClothArea,
                    Process_Dust = entity.Process_Dust,
                    Process_Dustload_gmspm3 = entity.Process_Dustload_gmspm3,
                    Process_Temp_C = entity.Process_Temp_C,
                    Dew_Point_C = entity.Dew_Point_C,
                    Outlet_Emission_mgpm3 = entity.Outlet_Emission_mgpm3,
                    Process_Cloth_Ratio = entity.Process_Cloth_Ratio,
                    Can_Correction = entity.Can_Correction,
                    Customer_Equipment_Tag_No = entity.Customer_Equipment_Tag_No,
                    Bagfilter_Cleaning_Type = entity.Bagfilter_Cleaning_Type,
                    Offline_Maintainence = entity.Offline_Maintainence,
                    Bag_Filter_Capacity_V = entity.Bag_Filter_Capacity_V,
                    Process_Vol_M3_Sec = entity.Process_Vol_M3_Sec,
                    Process_Vol_M3_Min = entity.Process_Vol_M3_Min,
                    Bag_Area = entity.Bag_Area,
                    Bag_Bottom_Area = entity.Bag_Bottom_Area,
                    Min_Cloth_Area_Req = entity.Min_Cloth_Area_Req,
                    Min_Bag_Req = entity.Min_Bag_Req,
                },

            };
        }

        public static ProcessInfo ToEntity(ProcessInfoMainDto dto)
        {
            if (dto == null) return null;
            return new ProcessInfo
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Process_Volume_M3h = dto.ProcessInfo.Process_Volume_M3h,
                Location = dto.ProcessInfo.Location,
                ProcessVolumeMin = dto.ProcessInfo.ProcessVolumeMin,
                Process_Acrmax = dto.ProcessInfo.Process_Acrmax,
                ClothArea = dto.ProcessInfo.ClothArea,
                Process_Dust = dto.ProcessInfo.Process_Dust,
                Process_Dustload_gmspm3 = dto.ProcessInfo.Process_Dustload_gmspm3,
                Process_Temp_C = dto.ProcessInfo.Process_Temp_C,
                Dew_Point_C = dto.ProcessInfo.Dew_Point_C,
                Outlet_Emission_mgpm3 = dto.ProcessInfo.Outlet_Emission_mgpm3,
                Process_Cloth_Ratio = dto.ProcessInfo.Process_Cloth_Ratio,
                Can_Correction = dto.ProcessInfo.Can_Correction,
                Customer_Equipment_Tag_No = dto.ProcessInfo.Customer_Equipment_Tag_No,
                Bagfilter_Cleaning_Type = dto.ProcessInfo.Bagfilter_Cleaning_Type,
                Offline_Maintainence = dto.ProcessInfo.Offline_Maintainence,
                Bag_Filter_Capacity_V = dto.ProcessInfo.Bag_Filter_Capacity_V,
                Process_Vol_M3_Sec = dto.ProcessInfo.Process_Vol_M3_Sec,
                Process_Vol_M3_Min = dto.ProcessInfo.Process_Vol_M3_Min,
                Bag_Area = dto.ProcessInfo.Bag_Area,
                Bag_Bottom_Area = dto.ProcessInfo.Bag_Bottom_Area,
                Min_Cloth_Area_Req = dto.ProcessInfo.Min_Cloth_Area_Req,
                Min_Bag_Req = dto.ProcessInfo.Min_Bag_Req,

            };
        }
    }
}
