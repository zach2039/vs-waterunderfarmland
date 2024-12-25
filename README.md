Water Under Farmland
=================

Overview
--------

This is a server-side mod that allows water under farmland to be a valid moisture source. 

Does not allow water to irrigate diagonally from below; water must be _directly below_ the farmland for it to be valid.


Known issues
--------

- Incompatible with mods that patch `BlockEntityFarmland.GetNearbyWaterDistance` or implement a custom farmland BlockEntity.
