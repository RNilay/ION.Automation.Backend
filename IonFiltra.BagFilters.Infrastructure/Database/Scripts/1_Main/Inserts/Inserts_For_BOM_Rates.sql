INSERT INTO ionfiltrabagfilters.BillOfMaterialRates (ItemKey, Rate, Unit) VALUES
  ('casing',                40.00,   'kg'),
  ('capsule',               40.00,   'kg'),
  ('hopper',                42.00,   'kg'),
  ('cage_ladder',          120.00,   'kg'),
  ('railing',              300.00,   'm'),
  ('tubesheet',             55.00,   'kg'),
  ('air_header',           150.00,   'kg'),
  ('structure',             35.00,   'kg'),
  ('hopper_access_stool', 86000.00,  'piece'),
  ('mid_landing_platform', 86000.00, 'piece'),
  ('maintenance_platform', 86000.00, 'piece'),
  ('cage',                 592.00,   'piece');
  ('scrap_holes',          35.00,   '');


  INSERT INTO ionfiltrabagfilters.PaintingCostConfig
    (Code, Section, Item, InrPerLtr, SqmPerLtr, Coats, LabourRate)
VALUES
    ('inside_primer', 'Material Cost Inside', 'Primer Cost', 70.00, 5.00, 2.00, NULL),

    ('outside_primer', 'Material Cost Outside', 'Primer Cost', 70.00, 5.00, 2.00, 1.25),
    ('outside_intermediate', 'Material Cost Outside', 'Intermediate Paint', 135.00, 5.00, 0.00, 0.00),
    ('outside_finish', 'Material Cost Outside', 'Finish Paint', 130.00, 5.00, 2.00, 1.25),

    ('outside_total', 'Material Cost Outside', 'Total Outside Painting', NULL, NULL, NULL, 2.00),
    ('total_labour', '', 'Total Labour', NULL, NULL, NULL, 4.50);
