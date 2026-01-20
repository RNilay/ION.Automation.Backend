using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Weight_Summary
{
    public static class WeightSummaryMapper
    {
        public static WeightSummaryMainDto ToMainDto(WeightSummary entity)
        {
            if (entity == null) return null;
            return new WeightSummaryMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                WeightSummary = new WeightSummaryDto
                {
                    Casing_Weight = entity.Casing_Weight,
                    Capsule_Weight = entity.Capsule_Weight,
                    Tot_Weight_Per_Compartment = entity.Tot_Weight_Per_Compartment,
                    Hopper_Weight = entity.Hopper_Weight,
                    Weight_Of_Cage_Ladder = entity.Weight_Of_Cage_Ladder,
                    Railing_Weight = entity.Railing_Weight,
                    Tubesheet_Weight = entity.Tubesheet_Weight,
                    Air_Header_Blow_Pipe = entity.Air_Header_Blow_Pipe,
                    Hopper_Access_Stool_Weight = entity.Hopper_Access_Stool_Weight,
                    Weight_Of_Mid_Landing_Plt = entity.Weight_Of_Mid_Landing_Plt,
                    Weight_Of_Maintainence_Pltform = entity.Weight_Of_Maintainence_Pltform,
                    Cage_Weight = entity.Cage_Weight,
                    Structure_Weight = entity.Structure_Weight,
                    Scrap_Holes_Weight = entity.Scrap_Holes_Weight,
                    Total_Weight_Of_Pressure_Header = entity.Total_Weight_Of_Pressure_Header,
                    Weight_Total = entity.Weight_Total,
                },

            };
        }

        public static WeightSummary ToEntity(WeightSummaryMainDto dto)
        {
            if (dto == null) return null;
            return new WeightSummary
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Casing_Weight = dto.WeightSummary.Casing_Weight,
                Capsule_Weight = dto.WeightSummary.Capsule_Weight,
                Tot_Weight_Per_Compartment = dto.WeightSummary.Tot_Weight_Per_Compartment,
                Hopper_Weight = dto.WeightSummary.Hopper_Weight,
                Weight_Of_Cage_Ladder = dto.WeightSummary.Weight_Of_Cage_Ladder,
                Railing_Weight = dto.WeightSummary.Railing_Weight,
                Tubesheet_Weight = dto.WeightSummary.Tubesheet_Weight,
                Air_Header_Blow_Pipe = dto.WeightSummary.Air_Header_Blow_Pipe,
                Hopper_Access_Stool_Weight = dto.WeightSummary.Hopper_Access_Stool_Weight,
                Weight_Of_Mid_Landing_Plt = dto.WeightSummary.Weight_Of_Mid_Landing_Plt,
                Weight_Of_Maintainence_Pltform = dto.WeightSummary.Weight_Of_Maintainence_Pltform,
                Cage_Weight = dto.WeightSummary.Cage_Weight,
                Structure_Weight = dto.WeightSummary.Structure_Weight,
                Scrap_Holes_Weight = dto.WeightSummary.Scrap_Holes_Weight,
                Total_Weight_Of_Pressure_Header = dto.WeightSummary.Total_Weight_Of_Pressure_Header,
                Weight_Total = dto.WeightSummary.Weight_Total,

            };
        }
    }
}
