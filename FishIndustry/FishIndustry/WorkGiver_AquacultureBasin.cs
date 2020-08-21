﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;   // Always needed
using RimWorld;      // RimWorld specific functions are found here
using Verse;         // RimWorld universal objects are here
using Verse.AI;      // Needed when you do something with the AI
//using Verse.Sound; // Needed when you do something with the Sound

namespace FishIndustry
{
    /// <summary>
    /// WorkGiver_AquacultureBasin class.
    /// </summary>
    /// <author>Rikiki</author>
    /// <permission>Use this code as you want, just remember to add a link to the corresponding Ludeon forum mod release thread.</permission>
    public class WorkGiver_AquacultureBasin : WorkGiver_Scanner
    {
		public override ThingRequest PotentialWorkThingRequest
		{
			get
            {
                return ThingRequest.ForDef(Util_FishIndustry.AquacultureBasinDef);
			}
		}

        public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
            if ((t is Building_AquacultureBasin) == false)
            {
                return false;
            }
            Building_AquacultureBasin aquacultureBasin = t as Building_AquacultureBasin;

            if (pawn.Dead
                || pawn.Downed
                || pawn.IsBurning())
            {
                return false;
            }
            if (pawn.CanReserveAndReach(aquacultureBasin, this.PathEndMode, Danger.Some) == false)
            {
                return false;
            }

            if (aquacultureBasin.fishIsHarvestable
                || aquacultureBasin.speciesShouldBeChanged
                || aquacultureBasin.shouldBeMaintained)
            {
                return true;
            }
            return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
            Job job = null;
            Building_AquacultureBasin aquacultureBasin = t as Building_AquacultureBasin;

            if (aquacultureBasin.fishIsHarvestable)
            {
                job = JobMaker.MakeJob(Util_FishIndustry.AquacultureBasinHarvestJobDef, aquacultureBasin);
            }
            else if (aquacultureBasin.speciesShouldBeChanged)
            {
                job = JobMaker.MakeJob(Util_FishIndustry.AquacultureBasinChangeSpeciesJobDef, aquacultureBasin);
            }
            else if (aquacultureBasin.shouldBeMaintained)
            {
                job = JobMaker.MakeJob(Util_FishIndustry.AquacultureBasinMaintainJobDef, aquacultureBasin);
            }
            return job;
		}
    }
}
