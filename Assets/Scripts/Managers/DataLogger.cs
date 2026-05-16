using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace VRHapticsExperiment
{
    public class DataLogger : MonoBehaviour
    {
        [SerializeField] private string filePrefix = "vr_haptics";

        private readonly List<string> hitRows = new();
        private string timestamp;

        private void Awake()
        {
            timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            hitRows.Add(
                "participantId,conditionId,attackType,targetMaterial,hapticPattern,targetName,trialTime,hitX,hitY,hitZ,velX,velY,velZ,speed,isValidHit,absoluteTime"
            );
        }

        public void LogHit(HitContext ctx)
        {
            string row = string.Join(",",
                Q(ctx.participantId),
                Q(ctx.conditionId),
                ctx.attackType,
                ctx.targetMaterial,
                ctx.hapticPattern,
                Q(ctx.targetName),
                ctx.trialTime.ToString("F3"),
                ctx.hitPoint.x.ToString("F4"),
                ctx.hitPoint.y.ToString("F4"),
                ctx.hitPoint.z.ToString("F4"),
                ctx.weaponVelocity.x.ToString("F4"),
                ctx.weaponVelocity.y.ToString("F4"),
                ctx.weaponVelocity.z.ToString("F4"),
                ctx.speed.ToString("F4"),
                ctx.isValidHit ? "1" : "0",
                Time.realtimeSinceStartup.ToString("F3")
            );

            hitRows.Add(row);
        }

        public void FlushAll()
        {
            string dir = Application.persistentDataPath;

            File.WriteAllText(
                Path.Combine(dir, $"{filePrefix}_hits_{timestamp}.csv"),
                Build(hitRows),
                Encoding.UTF8
            );

            Debug.Log($"Saved CSV logs to: {dir}");
        }

        private string Build(List<string> rows)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
                sb.AppendLine(row);
            return sb.ToString();
        }

        private string Q(string s)
        {
            if (string.IsNullOrEmpty(s)) return "\"\"";
            return $"\"{s.Replace("\"", "\"\"")}\"";
        }

        private void OnApplicationQuit()
        {
            FlushAll();
        }
    }
}