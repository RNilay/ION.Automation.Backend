CREATE OR REPLACE VIEW ionfiltrabagfilters.vw_BagfilterDetails AS
WITH DistinctPI AS (
    SELECT DISTINCT
        EnquiryId,
        BagfilterMasterId,
        Process_Volume_M3h
    FROM ionfiltrabagfilters.ProcessInfo
    WHERE Process_Volume_M3h IS NOT NULL
)
SELECT
    -- Enquiry
    E.Id                                 AS EnquiryId,
    E.UserId                             AS Enquiry_UserId,
    E.EnquiryId                          AS Enquiry_ExternalId,
    E.Customer                           AS Enquiry_Customer,
    E.RequiredBagFilters                 AS Enquiry_RequiredBagFilters,
    E.ProcessVolumes                     AS Enquiry_ProcessVolumes,

    -- BagfilterMaster
    BM.BagfilterMasterId                 AS BagfilterMaster_BagfilterMasterId,
    BM.AssignmentId                      AS BagfilterMaster_AssignmentId,
    BM.EnquiryId                         AS BagfilterMaster_EnquiryId,
    BM.BagFilterName                     AS BagfilterMaster_BagFilterName,
    BM.Status                            AS BagfilterMaster_Status,
    BM.Revision                          AS BagfilterMaster_Revision,

    -- WeightSummary
    WS.Id                                AS WeightSummary_Id,
    WS.EnquiryId                         AS WeightSummary_EnquiryId,
    WS.BagfilterMasterId                 AS WeightSummary_BagfilterMasterId,
    WS.Casing_Weight,
    WS.Capsule_Weight,
    WS.Tot_Weight_Per_Compartment,
    WS.Hopper_Weight,
    WS.Weight_Of_Cage_Ladder,
    WS.Railing_Weight,
    WS.Tubesheet_Weight                  AS WeightSummary_Tubesheet_Weight,
    WS.Air_Header_Blow_Pipe,
    WS.Hopper_Access_Stool_Weight,
    WS.Weight_Of_Mid_Landing_Plt,
    WS.Weight_Of_Maintainence_Pltform,
    WS.Cage_Weight                       AS WeightSummary_Cage_Weight,
    WS.Structure_Weight,
    WS.Weight_Total                      AS WeightSummary_Weight_Total,

    -- ProcessInfo (joined for the distinct Process_Volume_M3h)
    PI.Id                                AS ProcessInfo_Id,
    PI.EnquiryId                         AS ProcessInfo_EnquiryId,
    PI.BagfilterMasterId                 AS ProcessInfo_BagfilterMasterId,
    PI.Process_Volume_M3h,
    PI.Location,
    PI.ProcessVolumeMin,
    PI.Process_Acrmax,
    PI.ClothArea,
    PI.Process_Dust,
    PI.Process_Dustload_gmspm3,
    PI.Process_Temp_C,
    PI.Dew_Point_C,
    PI.Outlet_Emission_mgpm3,
    PI.Process_Cloth_Ratio,
    PI.Specific_Gravity,
    PI.Customer_Equipment_Tag_No,
    PI.Bagfilter_Cleaning_Type,
    PI.Offline_Maintainence,
    PI.Bag_Filter_Capacity_V,
    PI.Process_Vol_M3_Sec,
    PI.Process_Vol_M3_Min,
    PI.Bag_Area,
    PI.Bag_Bottom_Area,
    PI.Min_Cloth_Area_Req,
    PI.Min_Bag_Req,

    -- CageInputs
    CI.Id                                AS CageInputs_Id,
    CI.EnquiryId                         AS CageInputs_EnquiryId,
    CI.BagfilterMasterId                 AS CageInputs_BagfilterMasterId,
    CI.Cage_Wire_Dia,
    CI.No_Of_Cage_Wires,
    CI.Ring_Spacing,

    -- BagSelection
    BS.Id                                AS BagSelection_Id,
    BS.EnquiryId                         AS BagSelection_EnquiryId,
    BS.BagfilterMasterId                 AS BagSelection_BagfilterMasterId,
    BS.Filter_Bag_Dia,
    BS.Fil_Bag_Length,
    BS.ClothAreaPerBag,
    BS.noOfBags,
    BS.Fil_Bag_Recommendation,
    BS.Bag_Per_Row,
    BS.Number_Of_Rows,
    BS.Actual_Bag_Req,
    BS.Wire_Cross_Sec_Area,
    BS.No_Of_Rings,
    BS.Tot_Wire_Length,
    BS.Cage_Weight                       AS BagSelection_Cage_Weight,

    -- StructureInputs
    SI.Id                                AS StructureInputs_Id,
    SI.EnquiryId                         AS StructureInputs_EnquiryId,
    SI.BagfilterMasterId                 AS StructureInputs_BagfilterMasterId,
    SI.Gas_Entry,
    SI.Support_Structure_Type,
    SI.Can_Correction,
    SI.Nominal_Width,
    SI.Max_Bags_And_Pitch,
    SI.Nominal_Width_Meters,
    SI.Nominal_Length,
    SI.Nominal_Length_Meters,
    SI.Area_Adjust_Can_Vel,
    SI.Can_Area_Req,
    SI.Total_Avl_Area,
    SI.Length_Correction,
    SI.Length_Correction_Derived,
    SI.Actual_Length,
    SI.Actual_Length_Meters,
    SI.Ol_Flange_Length,
    SI.Ol_Flange_Length_Mm,

    -- CapsuleInputs
    CAPS.Id                              AS CapsuleInputs_Id,
    CAPS.EnquiryId                       AS CapsuleInputs_EnquiryId,
    CAPS.BagfilterMasterId               AS CapsuleInputs_BagfilterMasterId,
    CAPS.Valve_Size,
    CAPS.Voltage_Rating,
    CAPS.Cage_Type,
    CAPS.Cage_Length,
    CAPS.Capsule_Height,
    CAPS.Tube_Sheet_Thickness,
    CAPS.Capsule_Wall_Thickness,
    CAPS.Canopy,
    CAPS.Solenoid_Valve_Maintainence,
    CAPS.Capsule_Area,
    CAPS.Capsule_Weight                 AS CapsuleInputs_Capsule_Weight,
    CAPS.Tubesheet_Area,
    CAPS.Tubesheet_Weight,

    -- CasingInputs
    CAS.Id                               AS CasingInputs_Id,
    CAS.EnquiryId                        AS CasingInputs_EnquiryId,
    CAS.BagfilterMasterId                AS CasingInputs_BagfilterMasterId,
    CAS.Casing_Wall_Thickness,
    CAS.Casing_Height,
    CAS.Casing_Area,
    CAS.Casing_Weight                    AS CasingInputs_Casing_Weight,

    -- HopperInputs
    HOP.Id                               AS HopperInputs_Id,
    HOP.EnquiryId                        AS HopperInputs_EnquiryId,
    HOP.BagfilterMasterId                AS HopperInputs_BagfilterMasterId,
    HOP.Hopper_Type,
    HOP.Process_Compartments,
    HOP.Tot_No_Of_Hoppers,
    HOP.Tot_No_Of_Trough,
    HOP.Plenum_Width,
    HOP.Inlet_Height,
    HOP.Hopper_Thickness,
    HOP.Hopper_Valley_Angle,
    HOP.Access_Door_Type,
    HOP.Access_Door_Qty,
    HOP.Rav_Maintainence_Pltform,
    HOP.Hopper_Access_Stool,
    HOP.Is_Distance_Piece,
    HOP.Distance_Piece_Height,
    HOP.Stiffening_Factor,
    HOP.Hopper,
    HOP.Discharge_Opening_Sqr,
    HOP.Material_Handling,
    HOP.Material_Handling_Qty,
    HOP.Trough_Outlet_Length,
    HOP.Trough_Outlet_Width,
    HOP.Material_Handling_Xxx,
    HOP.Hor_Diff_Length,
    HOP.Hor_Diff_Width,
    HOP.Slant_Offset_Dist,
    HOP.Hopper_Height,
    HOP.Hopper_Height_Mm,
    HOP.Slanting_Hopper_Height,
    HOP.Hopper_Area_Length,
    HOP.Hopper_Area_Width,
    HOP.Hopper_Tot_Area,
    HOP.Hopper_Weight                     AS HopperInputs_Hopper_Weight,

    -- SupportStructure
    SS.Id                                AS SupportStructure_Id,
    SS.EnquiryId                         AS SupportStructure_EnquiryId,
    SS.BagfilterMasterId                 AS SupportStructure_BagfilterMasterId,
    SS.Support_Struct_Type,
    SS.NoOfColumn,
    SS.Column_Height,
    SS.Ground_Clearance,
    SS.Dist_Btw_Column_In_X,
    SS.Dist_Btw_Column_In_Z,
    SS.No_Of_Bays_In_X,
    SS.No_Of_Bays_In_Z,

    -- AccessGroup
    AG.Id                                AS AccessGroup_Id,
    AG.EnquiryId                         AS AccessGroup_EnquiryId,
    AG.BagfilterMasterId                 AS AccessGroup_BagfilterMasterId,
    AG.Access_Type,
    AG.Cage_Weight_Ladder,
    AG.Mid_Landing_Pltform,
    AG.Platform_Weight,
    AG.Staircase_Height,
    AG.Staircase_Weight,
    AG.Railing_Weight                     AS AccessGroup_Railing_Weight,
    AG.Maintainence_Pltform,
    AG.Maintainence_Pltform_Weight,
    AG.BlowPipe,
    AG.PressureHeader,
    AG.DistancePiece,
    AG.Access_Stool_Size_Mm,
    AG.Access_Stool_Size_Kg,

    -- RoofDoor
    RD.Id                                AS RoofDoor_Id,
    RD.EnquiryId                         AS RoofDoor_EnquiryId,
    RD.BagfilterMasterId                 AS RoofDoor_BagfilterMasterId,
    RD.Roof_Door_Thickness,
    RD.T2d,
    RD.T3d,
    RD.N_Doors,
    RD.Compartment_No,
    RD.Stiffness_Factor_For_Roof_Door,
    RD.Weight_Per_Door,
    RD.Tot_Weight_Per_Compartment         AS RoofDoor_Tot_Weight_Per_Compartment

FROM ionfiltrabagfilters.BagfilterMaster BM
LEFT JOIN ionfiltrabagfilters.Enquiry E
    ON BM.EnquiryId = E.Id

/* Distinct process volumes per enquiry+bagfilter */
LEFT JOIN DistinctPI D
    ON D.BagfilterMasterId = BM.BagfilterMasterId
    AND D.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

-- now join ProcessInfo rows that match that distinct volume (this will pick the PI row(s)
-- for the specific Process_Volume_M3h so PI columns reflect that volume)
LEFT JOIN ionfiltrabagfilters.ProcessInfo PI
    ON PI.BagfilterMasterId = BM.BagfilterMasterId
    AND PI.EnquiryId = COALESCE(BM.EnquiryId, E.Id)
    AND PI.Process_Volume_M3h = D.Process_Volume_M3h

LEFT JOIN ionfiltrabagfilters.WeightSummary WS
    ON WS.BagfilterMasterId = BM.BagfilterMasterId
    AND WS.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.CageInputs CI
    ON CI.BagfilterMasterId = BM.BagfilterMasterId
    AND CI.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.BagSelection BS
    ON BS.BagfilterMasterId = BM.BagfilterMasterId
    AND BS.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.StructureInputs SI
    ON SI.BagfilterMasterId = BM.BagfilterMasterId
    AND SI.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.CapsuleInputs CAPS
    ON CAPS.BagfilterMasterId = BM.BagfilterMasterId
    AND CAPS.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.CasingInputs CAS
    ON CAS.BagfilterMasterId = BM.BagfilterMasterId
    AND CAS.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.HopperInputs HOP
    ON HOP.BagfilterMasterId = BM.BagfilterMasterId
    AND HOP.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.SupportStructure SS
    ON SS.BagfilterMasterId = BM.BagfilterMasterId
    AND SS.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.AccessGroup AG
    ON AG.BagfilterMasterId = BM.BagfilterMasterId
    AND AG.EnquiryId = COALESCE(BM.EnquiryId, E.Id)

LEFT JOIN ionfiltrabagfilters.RoofDoor RD
    ON RD.BagfilterMasterId = BM.BagfilterMasterId
    AND RD.EnquiryId = COALESCE(BM.EnquiryId, E.Id);