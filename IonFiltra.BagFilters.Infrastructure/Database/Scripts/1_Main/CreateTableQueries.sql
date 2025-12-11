
---Enquiry table

CREATE TABLE Enquiry (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    EnquiryId VARCHAR(255) NOT NULL,
    Customer TEXT,
    RequiredBagFilters INT,
    ProcessVolumes JSON,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE (EnquiryId)
);

--Assignment table
CREATE TABLE AssignmentEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EnquiryId VARCHAR(255) NOT NULL,
    EnquiryAssignmentId TEXT,
    Customer TEXT,
    ProcessVolumes INT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry (EnquiryId) ON DELETE CASCADE
);

CREATE TABLE
    ionfiltrabagfilters.BagfilterMaster (
        BagfilterMasterId INT AUTO_INCREMENT PRIMARY KEY,
        AssignmentId INT NULL,
        EnquiryId INT NULL,
        BagFilterName TEXT,
        Status TEXT NULL,
        Revision INT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
    );



    CREATE TABLE
    ionfiltrabagfilters.BagfilterInput (
        BagfilterInputId INT AUTO_INCREMENT PRIMARY KEY,
        BagfilterMasterId INT NOT NULL,
        EnquiryId INT NULL,
        Process_Volume_M3h DECIMAL(10, 2),
        Location TEXT,
        Process_Dust TEXT,
        Process_Dustload_Gmspm3 DECIMAL(10, 2),
        Process_Temp_C DECIMAL(10, 2),
        Dew_Point_C DECIMAL(10, 2),
        Outlet_Emission_Mgpm3 TEXT,
        Process_Cloth_Ratio DECIMAL(10, 2),
        Can_Correction DECIMAL(10, 2),
        Customer_Equipment_Tag_No TEXT,
        Bagfilter_Cleaning_Type TEXT,
        Offline_Maintainence TEXT,
        Cage_Type TEXT,
        Cage_Sub_Type TEXT,
        Cage_Wire_Dia DECIMAL(10, 2),
        No_Of_Cage_Wires DECIMAL(10, 2),
        Ring_Spacing DECIMAL(10, 2),
        Cage_Diameter DECIMAL(10, 2),
        Cage_Length DECIMAL(10, 2),
        Cage_Configuration TEXT,
        Filter_Bag_Dia DECIMAL(10, 2),
        Fil_Bag_Length DECIMAL(10, 2),
        Fil_Bag_Recommendation TEXT,
        Gas_Entry TEXT,
        Support_Structure_Type TEXT,
        
        Valve_Size DECIMAL(10, 2),
        Voltage_Rating TEXT,
        Capsule_Height DECIMAL(10, 2),
        Tube_Sheet_Thickness DECIMAL(10, 2),
        Capsule_Wall_Thickness DECIMAL(10, 2),
        Canopy TEXT,
        Solenoid_Valve_Maintainence TEXT,
        Casing_Wall_Thickness DECIMAL(10, 2),
        Hopper_Type TEXT,
        Process_Compartments DECIMAL(10, 2),
        Tot_No_Of_Hoppers DECIMAL(10, 2),
        Tot_No_Of_Trough DECIMAL(10, 2),
        Plenum_Width DECIMAL(10, 2),
        Inlet_Height DECIMAL(10, 2),
        Hopper_Thickness DECIMAL(10, 2),
        Hopper_Valley_Angle DECIMAL(10, 2),
        Access_Door_Type TEXT,
        Access_Door_Qty DECIMAL(10, 2),
        Rav_Maintainence_Pltform TEXT,
        Hopper_Access_Stool TEXT,
        Is_Distance_Piece TEXT,
        Distance_Piece_Height DECIMAL(10, 2),
        Stiffening_Factor DECIMAL(10, 2),
        Hopper DECIMAL(10, 2),
        Discharge_Opening_Sqr DECIMAL(10, 2),
        Rav_Height DECIMAL(10, 2),
        Material_Handling TEXT,
        Material_Handling_Qty DECIMAL(10, 2),
        Trough_Outlet_Length DECIMAL(10, 2),
        Trough_Outlet_Width DECIMAL(10, 2),
        Material_Handling_XXX TEXT,
        Support_Struct_Type TEXT,
        No_Of_Column DECIMAL(10, 2),
        Ground_Clearance DECIMAL(10, 2),
        Slide_Gate_Height DECIMAL(10, 2),
        Access_Type TEXT,
        Cage_Weight_Ladder DECIMAL(10, 2),
        Mid_Landing_Pltform TEXT,
        Platform_Weight DECIMAL(10, 2),
        Staircase_Height DECIMAL(10, 2),
        Staircase_Weight DECIMAL(10, 2),
        Railing_Weight DECIMAL(10, 2),
        Maintainence_Pltform TEXT,
        Maintainence_Pltform_Weight DECIMAL(10, 2),
        Blow_Pipe DECIMAL(10, 2),
        Pressure_Header DECIMAL(10, 2),
        Distance_Piece DECIMAL(10, 2),
        Access_Stool_Size_Mm DECIMAL(10, 2),
        Access_Stool_Size_Kg DECIMAL(10, 2),
        Roof_Door_Thickness DECIMAL(10, 2),
        Column_Height DECIMAL(10, 2),
        Bag_Per_Row DECIMAL(10, 2),
        Number_Of_Rows DECIMAL(10, 2),
        IsMatched BOOLEAN NOT NULL DEFAULT 0,
        MatchedBagfilterInputId INT NULL,
        MatchedBagfilterMasterId INT NULL,
        MatchedAt DATETIME NULL,
        S3dModel JSON NULL,
        AnalysisResult JSON NULL,
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.BagfilterMaster (BagfilterMasterId) ON DELETE CASCADE
    );

    -- index for the above table:
    CREATE INDEX IX_BagfilterInputs_CompositeKey
ON BagfilterInput (Location(100), No_Of_Column, Ground_Clearance, Bag_Per_Row, Number_Of_Rows);


------ Sections tabs tables----

CREATE TABLE ionfiltrabagfilters.WeightSummary (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EnquiryId INT NOT NULL,
    BagfilterMasterId INT NOT NULL,
    Casing_Weight DECIMAL(10,2) ,
    Capsule_Weight DECIMAL(10,2) ,
    Tot_Weight_Per_Compartment DECIMAL(10,2) ,
    Hopper_Weight DECIMAL(10,2) ,
    Weight_Of_Cage_Ladder DECIMAL(10,2) ,
    Railing_Weight DECIMAL(10,2) ,
    Tubesheet_Weight DECIMAL(10,2) ,
    Air_Header_Blow_Pipe DECIMAL(10,2) ,
    Hopper_Access_Stool_Weight DECIMAL(10,2) ,
    Weight_Of_Mid_Landing_Plt DECIMAL(10,2) ,
    Weight_Of_Maintainence_Pltform DECIMAL(10,2) ,
    Cage_Weight DECIMAL(10,2) ,
    Structure_Weight DECIMAL(10,2) ,
    Weight_Total DECIMAL(10,2),
	CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
    FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
);
    
CREATE TABLE
    ionfiltrabagfilters.ProcessInfo (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Process_Volume_M3h DECIMAL(10, 2),
        Location TEXT,
        ProcessVolumeMin DECIMAL(10, 2),
        Process_Acrmax DECIMAL(10, 2),
        ClothArea DECIMAL(10, 2),
        Process_Dust TEXT,
        Process_Dustload_gmspm3 DECIMAL(10, 2),
        Process_Temp_C DECIMAL(10, 2),
        Dew_Point_C DECIMAL(10, 2),
        Outlet_Emission_mgpm3 TEXT,
        Process_Cloth_Ratio DECIMAL(10, 2),
        Can_Correction DECIMAL(10, 2),
        Customer_Equipment_Tag_No TEXT,
        Bagfilter_Cleaning_Type TEXT,
        Offline_Maintainence TEXT,
        Bag_Filter_Capacity_V DECIMAL(10, 2),
        Process_Vol_M3_Sec DECIMAL(10, 2),
        Process_Vol_M3_Min DECIMAL(10, 2),
        Bag_Area DECIMAL(10, 2),
        Bag_Bottom_Area DECIMAL(10, 2),
        Min_Cloth_Area_Req DECIMAL(10, 2),
        Min_Bag_Req DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.CageInputs (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Cage_Type TEXT,
        Cage_Sub_Type TEXT,
        Cage_Wire_Dia DECIMAL(10, 2),
        No_Of_Cage_Wires DECIMAL(10, 2),
        Ring_Spacing DECIMAL(10, 2),
        Cage_Diameter DECIMAL(10, 2),
        Cage_Length DECIMAL(10, 2),
        Cage_Configuration TEXT,
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.BagSelection (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Filter_Bag_Dia DECIMAL(10, 2),
        Fil_Bag_Length DECIMAL(10, 2),
        ClothAreaPerBag DECIMAL(10, 2),
        noOfBags DECIMAL(10, 2),
        Fil_Bag_Recommendation TEXT,
        Bag_Per_Row DECIMAL(10, 2),
        Number_Of_Rows DECIMAL(10, 2),
        Actual_Bag_Req DECIMAL(10, 2),
        Wire_Cross_Sec_Area DECIMAL(10, 2),
        No_Of_Rings DECIMAL(10, 2),
        Tot_Wire_Length DECIMAL(10, 2),
        Cage_Weight DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.StructureInputs (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Gas_Entry TEXT,
        Support_Structure_Type TEXT,
       
        Nominal_Width DECIMAL(10, 2),
        Max_Bags_And_Pitch DECIMAL(10, 2),
        Nominal_Width_Meters DECIMAL(10, 2),
        Nominal_Length DECIMAL(10, 2),
        Nominal_Length_Meters DECIMAL(10, 2),
        Area_Adjust_Can_Vel DECIMAL(10, 2),
        Can_Area_Req DECIMAL(10, 2),
        Total_Avl_Area DECIMAL(10, 2),
        Length_Correction DECIMAL(10, 2),
        Length_Correction_Derived DECIMAL(10, 2),
        Actual_Length DECIMAL(10, 2),
        Actual_Length_Meters DECIMAL(10, 2),
        Ol_Flange_Length DECIMAL(10, 2),
        Ol_Flange_Length_Mm DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.CapsuleInputs (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Valve_Size DECIMAL(10, 2),
        Voltage_Rating TEXT,
       
        Capsule_Height DECIMAL(10, 2),
        Tube_Sheet_Thickness DECIMAL(10, 2),
        Capsule_Wall_Thickness DECIMAL(10, 2),
        Canopy TEXT,
        Solenoid_Valve_Maintainence TEXT,
        Capsule_Area DECIMAL(10, 2),
        Capsule_Weight DECIMAL(10, 2),
        Tubesheet_Area DECIMAL(10, 2),
        Tubesheet_Weight DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.CasingInputs (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Casing_Wall_Thickness DECIMAL(10, 2),
        Casing_Height DECIMAL(10, 2),
        Casing_Area DECIMAL(10, 2),
        Casing_Weight DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.HopperInputs (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Hopper_Type TEXT,
        Process_Compartments DECIMAL(10, 2),
        Tot_No_Of_Hoppers DECIMAL(10, 2),
        Tot_No_Of_Trough DECIMAL(10, 2),
        Plenum_Width DECIMAL(10, 2),
        Inlet_Height DECIMAL(10, 2),
        Hopper_Thickness DECIMAL(10, 2),
        Hopper_Valley_Angle DECIMAL(10, 2),
        Access_Door_Type TEXT,
        Access_Door_Qty DECIMAL(10, 2),
        Rav_Maintainence_Pltform TEXT,
        Hopper_Access_Stool TEXT,
        Is_Distance_Piece TEXT,
        Distance_Piece_Height DECIMAL(10, 2),
        Stiffening_Factor DECIMAL(10, 2),
        Hopper DECIMAL(10, 2),
        Discharge_Opening_Sqr DECIMAL(10, 2),
        Rav_Height DECIMAL(10, 2),
        Material_Handling TEXT,
        Material_Handling_Qty DECIMAL(10, 2),
        Trough_Outlet_Length DECIMAL(10, 2),
        Trough_Outlet_Width DECIMAL(10, 2),
        Material_Handling_Xxx TEXT,
        Hor_Diff_Length DECIMAL(10, 2),
        Hor_Diff_Width DECIMAL(10, 2),
        Slant_Offset_Dist DECIMAL(10, 2),
        Hopper_Height DECIMAL(10, 2),
        Hopper_Height_Mm DECIMAL(10, 2),
        Slanting_Hopper_Height DECIMAL(10, 2),
        Hopper_Area_Length DECIMAL(10, 2),
        Hopper_Area_Width DECIMAL(10, 2),
        Hopper_Tot_Area DECIMAL(10, 2),
        Hopper_Weight DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.SupportStructure (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Support_Struct_Type TEXT,
        NoOfColumn DECIMAL(10, 2),
        Column_Height DECIMAL(10, 2),
        Ground_Clearance DECIMAL(10, 2),
        Slide_Gate_Height DECIMAL(10, 2),
        Dist_Btw_Column_In_X DECIMAL(10, 2),
        Dist_Btw_Column_In_Z DECIMAL(10, 2),
        No_Of_Bays_In_X DECIMAL(10, 2),
        No_Of_Bays_In_Z DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.AccessGroup (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Access_Type TEXT,
        Cage_Weight_Ladder DECIMAL(10, 2),
        Mid_Landing_Pltform TEXT,
        Platform_Weight DECIMAL(10, 2),
        Staircase_Height DECIMAL(10, 2),
        Staircase_Weight DECIMAL(10, 2),
        Railing_Weight DECIMAL(10, 2),
        Maintainence_Pltform TEXT,
        Maintainence_Pltform_Weight DECIMAL(10, 2),
        BlowPipe DECIMAL(10, 2),
        PressureHeader DECIMAL(10, 2),
        DistancePiece DECIMAL(10, 2),
        Access_Stool_Size_Mm DECIMAL(10, 2),
        Access_Stool_Size_Kg DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

    CREATE TABLE
    ionfiltrabagfilters.RoofDoor (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL,
        Roof_Door_Thickness DECIMAL(10, 2),
        T2d DECIMAL(10, 2),
        T3d DECIMAL(10, 2),
        N_Doors DECIMAL(10, 2),
        Compartment_No DECIMAL(10, 2),
        Stiffness_Factor_For_Roof_Door DECIMAL(10, 2),
        Weight_Per_Door DECIMAL(10, 2),
        Tot_Weight_Per_Compartment DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );

   CREATE TABLE
   ionfiltrabagfilters.PaintingArea (
     Id INT AUTO_INCREMENT PRIMARY KEY,
     EnquiryId INT NOT NULL,
     BagfilterMasterId INT NOT NULL,
     Inside_Area_Casing_Area_Mm2 DECIMAL(18, 2),
     Inside_Area_Casing_Area_M2 DECIMAL(18, 2),
     Inside_Area_Hopper_Area_Mm2 DECIMAL(18, 2),
     Inside_Area_Hopper_Area_M2 DECIMAL(18, 2),
     Inside_Area_Air_Header_Mm2 DECIMAL(18, 2),
     Inside_Area_Air_Header_M2 DECIMAL(18, 2),
     Inside_Area_Purge_Pipe_Mm2 DECIMAL(18, 2),
     Inside_Area_Purge_Pipe_M2 DECIMAL(18, 2),
     Inside_Area_Roof_Door_Mm2 DECIMAL(18, 2),
     Inside_Area_Roof_Door_M2 DECIMAL(18, 2),
     Inside_Area_Tube_Sheet_Mm2 DECIMAL(18, 2),
     Inside_Area_Tube_Sheet_M2 DECIMAL(18, 2),
     Inside_Area_Total_M2 DECIMAL(18, 2),
     Outside_Area_Casing_Area_Mm2 DECIMAL(18, 2),
     Outside_Area_Casing_Area_M2 DECIMAL(18, 2),
     Outside_Area_Hopper_Area_Mm2 DECIMAL(18, 2),
     Outside_Area_Hopper_Area_M2 DECIMAL(18, 2),
     Outside_Area_Air_Header_Mm2 DECIMAL(18, 2),
     Outside_Area_Air_Header_M2 DECIMAL(18, 2),
     Outside_Area_Purge_Pipe_Mm2 DECIMAL(18, 2),
     Outside_Area_Purge_Pipe_M2 DECIMAL(18, 2),
     Outside_Area_Roof_Door_Mm2 DECIMAL(18, 2),
     Outside_Area_Roof_Door_M2 DECIMAL(18, 2),
     Outside_Area_Tube_Sheet_Mm2 DECIMAL(18, 2),
     Outside_Area_Tube_Sheet_M2 DECIMAL(18, 2),
     Outside_Area_Total_M2 DECIMAL(18, 2),
     CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
     FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry (Id) ON DELETE CASCADE,
     FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
 );

 ---Bill of Material
 CREATE TABLE ionfiltrabagfilters.BillOfMaterial (
    Id               INT AUTO_INCREMENT PRIMARY KEY,
    EnquiryId INT NOT NULL,   -- FK to Enquiry row
    BagfilterMasterId INT NOT NULL, -- FK to BagfilterMaster row
    Item             VARCHAR(150) NOT NULL,
    Material         VARCHAR(50)  NULL,
    Weight           DECIMAL(18,2) NULL,
    Units            VARCHAR(20)  NULL,
    Rate             DECIMAL(18,2) NULL,
    Cost             DECIMAL(18,2) NULL,
    SortOrder        INT NULL,   -- to keep the same order as UI
    CreatedAt        DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt        DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry(Id) ON DELETE CASCADE,
    FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    
);

CREATE TABLE
    ionfiltrabagfilters.PaintingCost (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        EnquiryId INT NOT NULL,
        BagfilterMasterId INT NOT NULL, -- FK to BagfilterMaster row
        PaintingTableJson JSON,
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
        FOREIGN KEY (EnquiryId) REFERENCES ionfiltrabagfilters.Enquiry (Id) ON DELETE CASCADE,
        FOREIGN KEY (BagfilterMasterId) REFERENCES ionfiltrabagfilters.bagfiltermaster(BagfilterMasterId) ON DELETE CASCADE
    );


--- Master Table for Database of Without Canpoy Bagfilters

CREATE TABLE
  `IFI_Bagfilter_Database_Without_Canopy` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Process_Volume_m3hr` TEXT,
    `Hopper_type` TEXT,
    `Number_of_columns` DECIMAL(10, 2),
    `Number_of_bays_in_X_direction` DECIMAL(10, 2),
    `Number_of_bays_in_Y_direction` DECIMAL(10, 2),
    `Column_CC_distance_in_X_direction_mm` DECIMAL(10, 2),
    `Column_CC_distance_in_Y_direction_mm` DECIMAL(10, 2),
    `Clearance_Below_Hopper_Flange_mm` DECIMAL(10, 2),
    `Height_upto_mm_Column` DECIMAL(10, 2),
    `Height_upto_mm_Tube_Sheet` DECIMAL(10, 2),
    `Height_upto_mm_Capsule_Top` DECIMAL(10, 2),
    `Member_Sizes_Column` TEXT,
    `Member_Sizes_Beam` TEXT,
    `Member_Sizes_Bracing_and_Ties` TEXT,
    `Member_Sizes_RAV` TEXT,
    `Member_Sizes_Staging_Beam` TEXT,
    `Member_Sizes_Grid_Beam` TEXT,
    `Bolts_No_of_Bolt` DECIMAL(10, 2),
    `Bolts_Dia_of_Bolt` DECIMAL(10, 2),
    `Bolts_Grade_of_Bolt` DECIMAL(10, 2),
    `Bolts_Sleeve_Size_mm` TEXT,
    `Bolts_Embedded_Length_mm` DECIMAL(10, 2),
    `Bolt_CC_Distance_Confirguration_RCC` TEXT,
    `Bolt_CC_Distance_Confirguration_Steel` TEXT,
    `Base_Plate_Dimension_RCC` TEXT,
    `Base_Plate_Dimension_Steel` TEXT,
    `Weight_of_Base_Plate_kg` DECIMAL(10, 2),
    `Total_Weight_of_Structure_kg` DECIMAL(10, 2),
    `CreatedAt` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
  );

  CREATE TABLE
    ionfiltrabagfilters.IFI_Bagfilter_Database_With_Canopy (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        Process_Volume_m3hr TEXT,
        Hopper_type TEXT,
        Number_of_columns DECIMAL(10, 2),
        Number_of_bays_in_X_direction DECIMAL(10, 2),
        Number_of_bays_in_Y_direction DECIMAL(10, 2),
        Foot_Print_Column_CC_Header_Side_mm_x DECIMAL(10, 2),
        Foot_Print_Column_CC_Other_Side_mm_y DECIMAL(10, 2),
        Ht_of_Supp_Structure_mm_Hopper_Bottom DECIMAL(10, 2),
        Ht_of_Supp_Structure_mm_Column DECIMAL(10, 2),
        Ht_of_Supp_Structure_mm_Tube_Sheet DECIMAL(10, 2),
        Ht_of_Supp_Structure_mm_Capsule_Top DECIMAL(10, 2),
        Ht_of_Supp_Structure_mm_Shed_Height DECIMAL(10, 2),
        Member_Sizes_Column TEXT,
        Member_Sizes_Beam TEXT,
        Member_Sizes_Bracing_Ties TEXT,
        Member_Sizes_RAV TEXT,
        Member_Sizes_Staging_Beam TEXT,
        Member_Sizes_Grid_Beam TEXT,
        Member_Sizes_mm_Shed_Column_Rafter TEXT,
        Member_Sizes_mm_Shed_Girt_and_Purlin_along_X_axis TEXT,
        Member_Sizes_mm_Shed_Girt_Purlin_and_Ridge_along_Y_axis TEXT,
        Member_Sizes_mm_Shed_Shed_Bracings TEXT,
        Member_Sizes_mm_Shed_Roof_Truss TEXT,
        Bolts_No_of_Bolt DECIMAL(10, 2),
        Bolts_Dia_of_Bolt DECIMAL(10, 2),
        Bolts_Grade_of_Bolt DECIMAL(10, 2),
        Bolts_Sleeve_Size_mm TEXT,
        Bolts_Embedded_Length_mm DECIMAL(10, 2),
        Bolt_CC_Distance_Confirguration_RCC TEXT,
        Bolt_CC_Distance_Confirguration_Steel TEXT,
        Base_Plate_Dimension_RCC TEXT,
        Base_Plate_Dimension_Steel TEXT,
        Weight_of_Base_Plate_kg DECIMAL(10, 2),
        Total_Weight_of_Structure_kg DECIMAL(10, 2),
        Weight_of_Plates_kg DECIMAL(10, 2),
        CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
    );


  -----Bill Of Material Rates table

  CREATE TABLE ionfiltrabagfilters.BillOfMaterialRates (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ItemKey VARCHAR(50) NOT NULL,
    Rate DECIMAL(10,2) NOT NULL,
    Unit VARCHAR(20) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE ionfiltrabagfilters.PaintingCostConfig (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Code VARCHAR(50) NOT NULL UNIQUE,      -- e.g. 'inside_primer'
    Section VARCHAR(50) NOT NULL,          -- 'Material Cost Inside' / 'Material Cost Outside' / ''
    Item VARCHAR(100) NOT NULL,            -- 'Primer Cost', 'Intermediate Paint', etc.
    InrPerLtr DECIMAL(10,2) NULL,
    SqmPerLtr DECIMAL(10,2) NULL,
    Coats DECIMAL(10,2) NULL,
    LabourRate DECIMAL(10,2) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
);

----master data tables

CREATE TABLE ionfiltrabagfilters.FilterBag (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Material VARCHAR(255),
    Gsm DECIMAL(10, 2),
    Size VARCHAR(50),
    Make VARCHAR(100),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1)  DEFAULT 0
);



CREATE TABLE ionfiltrabagfilters.SolenoidValve (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(100) ,
    Size DECIMAL(10,2) ,
    Model VARCHAR(255) ,
    Cost DECIMAL(10,2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
    );
    

    CREATE TABLE ionfiltrabagfilters.DPTEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255) ,
    Model VARCHAR(500) ,
    Cost DECIMAL(10,2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
    );


    CREATE TABLE ionfiltrabagfilters.DPGEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);

CREATE TABLE ionfiltrabagfilters.DPSEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);

CREATE TABLE ionfiltrabagfilters.PGEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.PTEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.PSEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.HLDEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.RTDEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.UTubeManometer (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.ExplosionVent (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Size VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.FieldHooter (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Size VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);

CREATE TABLE ionfiltrabagfilters.AFREntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.ProxyPulser (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.ProxymitySwitch (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.CableEntity (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Size VARCHAR(500),
    Type VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.ZssController (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.JunctionBox (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.VibrationTransmitter (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Size VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.Thermocouple (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.Thermostat (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);



CREATE TABLE ionfiltrabagfilters.HopperHeatingPad (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.HopperHeatingcontroller (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(500),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.CentrifualFan(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Volume DECIMAL(10, 2),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.ScrewConveyor(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Length DECIMAL(10, 2),
    Width DECIMAL(10, 2),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.DragChainConveyor(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Length DECIMAL(10, 2),
    Width DECIMAL(10, 2),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.MotorisedActuator(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(255),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.RAVGearedMotor(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    KW VARCHAR(255),
    Make VARCHAR(255),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.HardwareEntity(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Item VARCHAR(255),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.SStubing(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Item VARCHAR(255),
    Material VARCHAR(255),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);



CREATE TABLE ionfiltrabagfilters.LTMotor (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    KW DECIMAL(10, 2),
    Efficiency DECIMAL(5, 2),
    FrameSize VARCHAR(100),
    RPM INT,
    Make VARCHAR(255),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.TimerEntity(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255),
    Model VARCHAR(255),
    Cost DECIMAL(10, 2),
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    IsDefault TINYINT(1) DEFAULT 0
);


CREATE TABLE ionfiltrabagfilters.MasterDefinitions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    MasterKey VARCHAR(100) NOT NULL,
    DisplayName VARCHAR(255) NOT NULL,
    ApiRoute VARCHAR(255),
    SectionOrder INT,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    ColumnsJson JSON,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL ON UPDATE CURRENT_TIMESTAMP
    );

