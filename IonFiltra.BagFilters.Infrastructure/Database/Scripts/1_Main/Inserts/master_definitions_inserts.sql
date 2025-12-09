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
    'Solenoid Valve',
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
    'DPT',
    'DPTEntity',           -- DB table name
    3,
    1,
    JSON_ARRAY(
        JSON_OBJECT('field', 'make',  'label', 'Make',  'type', 'string'),
        JSON_OBJECT('field', 'model', 'label', 'Model', 'type', 'string'),
        JSON_OBJECT('field', 'cost',  'label', 'Cost',  'type', 'number')
    )
);


