﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace AnimalControls
{
    //driver to make pawns wait until another pawn is finishied with thir current task
    public class JobDriver_PayAttention: JobDriver_Wait
    {
        public static void ForcePayAttention(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false)
        {
            if (!Settings.animals_pay_attention)
            {
                PawnUtility.ForceWait(pawn, ticks, faceTarget, maintainPosture);
                return;
            }
            //
            if (pawn.CurJob != null && pawn.CurJob.def == AnimalControlsDefOf.AnimalControls_Wait_PayAttention) return;
            //
            Job job = JobMaker.MakeJob(AnimalControlsDefOf.AnimalControls_Wait_PayAttention, faceTarget);
            pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false);
        }

        public Job payAttentinoTo = null;

        public override void DecorateWaitToil(Toil wait)
        {
            Pawn TargetOfAttention = TargetA.Thing as Pawn;
            if (TargetOfAttention == null) throw new Exception("Can't pay attention to a null value");
            payAttentinoTo = TargetOfAttention.CurJob;
            //Log.Message($"{TargetOfAttention},{payAttentinoTo}");
            base.DecorateWaitToil(wait);
            wait.AddFailCondition(() => payAttentinoTo == null || payAttentinoTo != (TargetA.Thing as Pawn).CurJob);
        }
    }
}
