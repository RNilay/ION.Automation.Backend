
---------------Bought-Out Items Inserts--------------------

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'filterBag',           -- frontend logical key
    'Filter Bag',          -- UI label
    'FilterBag',           -- DB table name => used as {viewName}/{tableName}
    1,                     -- section order in UI
    1,                     -- IsActive = true
    JSON_ARRAY(
        JSON_OBJECT('field', 'material', 'label', 'Material', 'type', 'string'),
        JSON_OBJECT('field', 'gsm',      'label', 'GSM',      'type', 'number'),
        JSON_OBJECT('field', 'size',     'label', 'Size',     'type', 'string'),
        JSON_OBJECT('field', 'make',     'label', 'Make',     'type', 'string'),
        JSON_OBJECT('field', 'cost',     'label', 'Cost',     'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'solenoidValve',
    'Solenoid Operated Double Diaphragm Valve',
    'SolenoidValve',       -- DB table name
    2,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'size',  'label', 'Size',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'dpt',
    'Differential Pressure Transmitter',
    'DPTEntity',           -- DB table name
    3,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'dpg',
    'Differential Pressure Gauge',
    'DPGEntity',
    4,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'dps',
    'Differential Pressure Switch',
    'DPSEntity',
    5,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'pg',
    'Pressure Gauge',
    'PGEntity',
    6,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'pt',
    'Pressure Transmitter',
    'PTEntity',
    7,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'ps',
    'Pressure Switch',
    'PSEntity',
    8,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'hld',
  'Hopper Level Detector',
  'HLDEntity',
  9,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'rtd',
  'Resistance Temperature Detector',
  'RTDEntity',
  10,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'utubeManometer',
  'U Tube Manometer',
  'UTubeManometer',
  11,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'explosionVent',
  'Explosion Vent',
  'ExplosionVent',
  12,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'fieldHooter',
  'Field Hooter',
  'FieldHooter',
  13,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'afr',
  'Air Filter Regulator',
  'AFREntity',
  14,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'proxyPulser',
  'Proxy Pulser',
  'ProxyPulser',
  15,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'proximitySwitch',
  'Proximity Switch',
  'ProxymitySwitch',
  16,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'cable',
  'Cable',
  'CableEntity',
  17,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','type','label','Type','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'zssController',
  'ZSS Controller',
  'ZssController',
  18,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'junctionBox',
  'Junction Box',
  'JunctionBox',
  19,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'vibrationTransmitter',
  'Vibration Transmitter',
  'VibrationTransmitter',
  20,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','size','label','Size','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'thermocouple',
  'Thermocouple',
  'Thermocouple',
  21,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(`MasterKey`,`DisplayName`,`ApiRoute`,`SectionOrder`,`IsActive`,`ColumnsJson`)
VALUES
(
  'thermostat',
  'Thermostat',
  'Thermostat',
  22,
  1,
  JSON_ARRAY(
    JSON_OBJECT('field','make','label','Make','type','string'),
    JSON_OBJECT('field','model','label','Model','type','string'),
    JSON_OBJECT('field','cost','label','Cost','type','number')
  )
);


INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'hopperHeatingPad',
    'Hopper Heating Pad',
    'HopperHeatingPad',
    23,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'hopperHeatingController',
    'Hopper Heating Controller',
    'HopperHeatingcontroller',
    24,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

--INSERT INTO ionfiltrabagfilters.MasterDefinitions
--(
--    `MasterKey`,
--    `DisplayName`,
--    `ApiRoute`,
--    `SectionOrder`,
--    `IsActive`,
--    `ColumnsJson`
--)
--VALUES
--(
--    'centrifugalFan',
--    'Centrifugal Fan',
--    'CentrifualFan',
--    25,
--    1,
--    JSON_ARRAY(
--        JSON_OBJECT('field', 'make',   'label', 'Make',   'type', 'string'),
--        JSON_OBJECT('field', 'volume', 'label', 'Volume', 'type', 'string'),
--        JSON_OBJECT('field', 'cost',   'label', 'Cost',   'type', 'number')
--    )
--);
--
--INSERT INTO ionfiltrabagfilters.MasterDefinitions
--(
--    `MasterKey`,
--    `DisplayName`,
--    `ApiRoute`,
--    `SectionOrder`,
--    `IsActive`,
--    `ColumnsJson`
--)
--VALUES
--(
--    'screwConveyor',
--    'Screw Conveyor',
--    'ScrewConveyor',
--    26,
--    1,
--    JSON_ARRAY(
--        JSON_OBJECT('field', 'make',   'label', 'Make',   'type', 'string'),
--        JSON_OBJECT('field', 'length', 'label', 'Length', 'type', 'string'),
--        JSON_OBJECT('field', 'width',  'label', 'Width',  'type', 'string'),
--        JSON_OBJECT('field', 'cost',   'label', 'Cost',   'type', 'number')
--    )
--);
--
--INSERT INTO ionfiltrabagfilters.MasterDefinitions
--(
--    `MasterKey`,
--    `DisplayName`,
--    `ApiRoute`,
--    `SectionOrder`,
--    `IsActive`,
--    `ColumnsJson`
--)
--VALUES
--(
--    'dragChainConveyor',
--    'Drag Chain Conveyor',
--    'DragChainConveyor',
--    27,
--    1,
--    JSON_ARRAY(
--        JSON_OBJECT('field', 'make',   'label', 'Make',   'type', 'string'),
--        JSON_OBJECT('field', 'length', 'label', 'Length', 'type', 'string'),
--        JSON_OBJECT('field', 'width',  'label', 'Width',  'type', 'string'),
--        JSON_OBJECT('field', 'cost',   'label', 'Cost',   'type', 'number')
--    )
--);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'motorisedActuator',
    'Motorised Actuator',
    'MotorisedActuator',
    28,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'ravGearedMotor',
    'RAV Geared Motor',
    'RAVGearedMotor',
    29,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'kw',   'label', 'KW',   'type', 'string'),
        JSON_OBJECT('field', 'make', 'label', 'Make', 'type', 'string'),
        JSON_OBJECT('field', 'cost', 'label', 'Cost', 'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'hardware',
    'Hardware',
    'HardwareEntity',
    30,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'item', 'label', 'Item', 'type', 'string'),
        JSON_OBJECT('field', 'cost', 'label', 'Cost', 'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'sstubing',
    'SS Tubing',
    'SStubing',
    31,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'item',     'label', 'Item',     'type', 'string'),
        JSON_OBJECT('field', 'material', 'label', 'Material', 'type', 'string'),
        JSON_OBJECT('field', 'cost',     'label', 'Cost',     'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'ltMotor',
    'LT Motor',
    'LTMotor',
    32,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'kw',         'label', 'KW',          'type', 'number'),
        JSON_OBJECT('field', 'efficiency', 'label', 'Efficiency',  'type', 'number'),
        JSON_OBJECT('field', 'frameSize',  'label', 'Frame Size',  'type', 'string'),
        JSON_OBJECT('field', 'rpm',        'label', 'RPM',         'type', 'number'),
        JSON_OBJECT('field', 'make',       'label', 'Make',        'type', 'string'),
        JSON_OBJECT('field', 'cost',       'label', 'Cost',        'type', 'number')
    )
);

INSERT INTO ionfiltrabagfilters.MasterDefinitions
(
    `MasterKey`,
    `DisplayName`,
    `ApiRoute`,
    `SectionOrder`,
    `IsActive`,
    `ColumnsJson`
)
VALUES
(
    'timer',
    'Timer',
    'TimerEntity',
    33,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);



----------------------Cage Costing config inserts -------------

INSERT INTO ionfiltrabagfilters.CageMaterialConfig
(Material, Density, CoilCost, TopBottomCost, VenturiCost, RivetCost)
VALUES
('GI', 7850, 61, 60, 70, 1.5),
('SS304', 7800, 215, 200, 70, 1.5);


INSERT INTO ionfiltrabagfilters.CageMiscellaneousConfig
(Item, Value, Unit)
VALUES
('Packing Raw Material', 0.7, 'Kg'),
('Square Tube for Packing', 60, 'INR'),
('Tarpoline', 2, 'INR/cage');

INSERT INTO ionfiltrabagfilters.AdminCostConfig
(Item, Value, Unit)
VALUES
('Cage Fabrication Cost', 10, 'INR/kg'),
('Scrap Rate', 35, 'INR/kg');

INSERT INTO ionfiltrabagfilters.StandardCageConfig
(CageSpec, WeightKg, RawMaterialCost, FixedFabricationCost, BoughtOutCost, PackingCost, PaintCost, TarpolineCost, FinalCostINR)
VALUES
('Cage 10 wire length: 3580 spacing: 150mm coil dia: 4mm', 4.91, 299.51, 65, 131.5, 42, 0.035, 2, 540),
('Cage 10 wire length: 3580 spacing: 175mm coil dia: 4mm', 4.42, 269.62, 55, 131.5, 42, 0.035, 2, 500),
('Cage 10 wire length: 3640 spacing: 150mm coil dia: 4mm', 4.64, 283.04, 65, 131.5, 42, 0.035, 2, 524),
('Cage 10 wire length: 3640 spacing: 175mm coil dia: 4mm', 4.49, 273.89, 65, 131.5, 42, 0.035, 2, 514),

('Cage 12 wire length: 3580 spacing: 150mm coil dia: 4mm', 5.27, 321.47, 78, 131.5, 42, 0.035, 2, 575),
('Cage 12 wire length: 3580 spacing: 175mm coil dia: 4mm', 5.12, 312.32, 69, 131.5, 42, 0.035, 2, 557),
('Cage 12 wire length: 3640 spacing: 150mm coil dia: 4mm', 5.36, 326.96, 78, 131.5, 42, 0.035, 2, 580),
('Cage 12 wire length: 3640 spacing: 175mm coil dia: 4mm', 5.21, 317.81, 78, 131.5, 42, 0.035, 2, 571),

('Cage 14 wire length: 3580 spacing: 175mm coil dia: 4mm', 5.83, 355.63, 70, 131.5, 42, 0.035, 2, 601),
('Cage 14 wire length: 3640 spacing: 150mm coil dia: 4mm', 6.08, 370.88, 91, 131.5, 42, 0.035, 2, 637),
('Cage 14 wire length: 3580 spacing: 150mm coil dia: 4mm', 5.98, 364.78, 91, 131.5, 42, 0.035, 2, 631),
('Cage 14 wire length: 3640 spacing: 175mm coil dia: 4mm', 5.93, 361.73, 91, 131.5, 42, 0.035, 2, 628);


-------------Transportation Rate Config------

INSERT INTO ionfiltrabagfilters.TransportationRateConfig
(LoadingFrom, DestinationState, RatePerKm_40x10x8, DistanceKm)
VALUES
('Pune', 'Andhra Pradesh', 130, 868),
('Pune', 'Arunachal Pradesh', 100, 3062),
('Pune', 'Assam', 100, 2708),
('Pune', 'Bihar', 100, 1717),
('Pune', 'Chhattisgarh', 98, 996),
('Pune', 'Dadra & Nagar Haveli',120, 272),
('Pune', 'Daman & Diu', 120, 300),
('Pune', 'Delhi', 104, 1462),
('Pune', 'Goa', 110, 469),
('Pune', 'Gujarat', 110, 739),
('Pune', 'Haryana', 105, 1561),
('Pune', 'Himachal Pradesh', 100, 1850),
('Pune', 'Manipur', 100, 3104),
('Pune', 'Jammu & Kashmir', 120, 2102),
('Pune', 'Jharkhand', 102, 1663),
('Pune', 'Karnataka', 125, 521),
('Pune', 'Kerala', 110, 1310),
('Pune', 'Madhya Pradesh', 125, 1077),
('Pune', 'Maharashtra', 115, 388),
('Pune', 'Meghalaya', 90, 2684),
('Pune', 'Nagaland', 90, 3118),
('Pune', 'Odisha', 100, 1445),
('Pune', 'Puducherry', 110, 1185),
('Pune', 'Punjab', 110, 1725),
('Pune', 'Rajasthan', 105, 1258),
('Pune', 'Sikkim', 100, 2289),
('Pune', 'Tamil Nadu', 120, 1166),
('Pune', 'Telangana', 120, 676),
('Pune', 'Uttarakhand', 120, 1791),
('Pune', 'Uttar Pradesh', 115, 1547),


('Katni', 'Andhra Pradesh', 130, 868),
('Katni', 'Arunachal Pradesh', 100, 3062),
('Katni', 'Assam', 100, 2708),
('Katni', 'Bihar', 100, 1717),
('Katni', 'Chhattisgarh', 98, 996),
('Katni', 'Dadra & Nagar Haveli',120, 272),
('Katni', 'Daman & Diu', 120, 300),
('Katni', 'Delhi', 104, 1462),
('Katni', 'Goa', 110, 469),
('Katni', 'Gujarat', 110, 739),
('Katni', 'Haryana', 105, 1561),
('Katni', 'Himachal Pradesh', 100, 1850),
('Katni', 'Manipur', 100, 3104),
('Katni', 'Jammu & Kashmir', 120, 2102),
('Katni', 'Jharkhand', 102, 1663),
('Katni', 'Karnataka', 125, 521),
('Katni', 'Kerala', 110, 1310),
('Katni', 'Madhya Pradesh', 125, 1077),
('Katni', 'Maharashtra', 115, 388),
('Katni', 'Meghalaya', 90, 2684),
('Katni', 'Nagaland', 90, 3118),
('Katni', 'Odisha', 100, 1445),
('Katni', 'Puducherry', 110, 1185),
('Katni', 'Punjab', 110, 1725),
('Katni', 'Rajasthan', 105, 1258),
('Katni', 'Sikkim', 100, 2289),
('Katni', 'Tamil Nadu', 120, 1166),
('Katni', 'Telangana', 120, 676),
('Katni', 'Uttarakhand', 120, 1791),
('Katni', 'Uttar Pradesh', 115, 1547);


------Damper Cost config

INSERT INTO ionfiltrabagfilters.DamperCostMiscellaneousConfig
(Item, Value, Unit)
VALUES
('Red Oxide', 90, '/L'),
('Enamel Paint', 150, '/L'),
('Damper Fab Cost', 30, '/kg'),
('MS Raw Material', 78, '/kg'),
('Hardware', 1000, '?');


INSERT INTO ionfiltrabagfilters.DamperSizesConfig
(Series, DiameterMm, ThicknessFactor, FinishedWeightKg, ScrapWeightKg, SurfaceAreaM2, PaintingCostPerKg)
VALUES
-- SMALL
('Small', 100, 1.05, 9, 9.45, 0.33, 8.00),
('Small', 125, 1.05, 10, 10.5, 0.41, 10.00),
('Small', 150, 1.05, 12, 12.6, 0.52, 12.00),
('Small', 200, 1.05, 18, 16.95, 0.73, 18.00),
('Small', 250, 1.035, 29, 30.015, 1.13, 27.12),
('Small', 300, 1.035, 36, 37.26, 1.45, 35.00),
('Small', 350, 1.035, 40, 41.4, 4.87, 117.00),
('Small', 400, 1.035, 48, 49.68, 5.83, 140.00),

-- MEDIUM
('Medium', 450, 0.99, 69, 68.31, 2.82, 68.00),
('Medium', 500, 0.99, 80, 79.2, 3.31, 79.00),
('Medium', 550, 0.99, 92, 91.08, 3.9, 94.00),
('Medium', 600, 0.99, 105, 103.95, 4.41, 106.00),
('Medium', 650, 0.99, 119, 117.81, 5.01, 120.00),
('Medium', 700, 0.99, 134, 132.66, 5.66, 136.00),

-- LARGE
('Large', 750, 0.949, 192, 182.21, 7.5, 180),
('Large', 800, 0.949, 211, 200.24, 8.28, 198.72),
('Large', 850, 0.949, 230, 218.27, 9.11, 218.64),
('Large', 900, 0.949, 251, 238.2, 9.99, 239.76),
('Large', 950, 0.949, 272, 258.13, 10.88, 261.12),
('Large', 1000, 0.949, 294, 279.01, 11.82, 283.68),
('Large', 1050, 0.949, 317, 300.83, 12.8, 307.2),
('Large', 1100, 0.949, 340, 322.66, 13.82, 331.68),
('Large', 1150, 0.949, 365, 346.39, 14.88, 357.12),
('Large', 1200, 0.949, 391, 371.06, 15.98, 383.52),
('Large', 1250, 0.949, 418, 396.68, 17.16, 411.84),
('Large', 1300, 0.949, 444, 421.36, 18.3, 439.2),
('Large', 1350, 0.949, 472, 447.93, 19.52, 468.48),
('Large', 1400, 0.949, 501, 475.45, 20.78, 498.72),
('Large', 1450, 0.949, 531, 503.92, 22.07, 529.68),
('Large', 1500, 0.949, 562, 533.34, 23.4, 561.6);


