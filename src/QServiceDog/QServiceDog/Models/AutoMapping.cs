using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace QServiceDog.Models
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //    CreateMap<IBand, BandLib.Band>();
            //    CreateMap<BandLib.Interfaces.IBLEGateway, BandLib.BLEGateway>();

            //    CreateMap<IBand, BI.EF.BandBase>()
            //    .ForMember(r => r.ID, opt => opt.MapFrom(src => Convert.ToUInt64(src.Mac.Replace(":", ""), 16)))
            //    .ForMember(r => r.ID5, opt => opt.MapFrom(src => 0xFFFFFFFFFFL & Convert.ToUInt64(src.Mac.Replace(":", ""), 16)))
            //    .ForMember(r => r.LastTrySyncTime, opt => opt.Condition(src => !src.LastTrySyncTime.LessThanMinDate()))
            //    .ForMember(r => r.LastConnectedTime, opt => opt.Condition(src => !src.LastConnectedTime.LessThanMinDate()))
            //    .ForMember(r => r.LastFindedTime, opt => opt.Condition(src => !src.LastFindedTime.LessThanMinDate()))
            //    .ForMember(r => r.LastTryWriteCodeTime, opt => opt.Condition(src => !src.LastTryWriteCodeTime.LessThanMinDate()))
            //    .ForMember(r => r.LastTryWriteMacTime, opt => opt.Condition(src => !src.LastTryWriteMacTime.LessThanMinDate()))
            //    .ForMember(r => r.CreateTime, opt => opt.MapFrom(src => DateTime.Now))
            //    .ForMember(r => r.CardSerNo, opt => opt.MapFrom(src => src.CardNo))
            //    ;
            //    CreateMap<IBand, BI.EF.Broadcast>()
            //    .ForMember(r => r.ID, opt => opt.MapFrom(src => Convert.ToUInt64(src.Mac.Replace(":", ""), 16)))
            //    .ForMember(r => r.Position, opt => opt.MapFrom(src => src.Position ?? ""))
            //    .ForMember(r => r.RSSI, opt => opt.MapFrom(src => (byte)Math.Abs(src.BroadcastData.RSSI))) //先使用最后一次收到的
            //   .ForMember(r => r.HeartRate, opt => opt.MapFrom(src => src.BroadcastData.HeartRate))
            //    .ForMember(r => r.Step, opt => opt.MapFrom(src => src.BroadcastData.Step))
            //    .ForMember(r => r.Battery, opt => opt.MapFrom(src => src.BroadcastData.Battery))
            //    .ForMember(r => r.Temperature, opt => opt.MapFrom(src => src.BroadcastData.Temperature))
            //    .ForMember(r => r.Calorie, opt => opt.MapFrom(src => src.BroadcastData.CAL))
            //    .ForMember(r => r.Time, opt => opt.MapFrom(src => src.BroadcastData.Time.GetLessThanMinDate()))
            //    .ForMember(r => r.Data, opt => opt.MapFrom(src => src.BroadcastData.ExtInfo))
            //    ;
            //    CreateMap<IBLEGateway, BI.EF.AP>()
            //    .ForMember(r => r.ID, opt => opt.MapFrom(src => Convert.ToUInt64(src.Mac.Replace(":", ""), 16)))
            //    .ForMember(r => r.RebootTime, opt => opt.Ignore())
            //    .ForMember(r => r.Pwd, opt => opt.MapFrom(src => src.Password))
            //    .ForMember(r => r.UpdateTime, opt => opt.MapFrom(src => DateTime.Now)).ReverseMap();
            //    ;


            //    CreateMap<BandLib.Structs.BandTrackData, BI.EF.Track>()
            //   .ForMember(r => r.ID, opt => opt.MapFrom(src => LongMac.long2mac(src.BandLong).ToSecondMac(src.StartTime).ID))
            //   .ForMember(r => r.BandID, opt => opt.MapFrom(src => src.BandLong))
            //   .ForMember(r => r.APID, opt => opt.MapFrom(src => src.APLong))
            //   .ForMember(r => r.LastRSSI, opt => opt.MapFrom(src => src.LastRSSI))
            //   .ForMember(r => r.MaxRSSI, opt => opt.MapFrom(src => src.MaxRSSI))
            //   .ForMember(r => r.StartTime, opt => opt.MapFrom(src => new DateTimeOffset(src.StartTime).ToUnixTimeSeconds()))
            //   .ForMember(r => r.EndTime, opt => opt.MapFrom(src => new DateTimeOffset(src.EndTime).ToUnixTimeSeconds()))
            //       ;
            //    CreateMap<BI.EF.Track, BandLib.Structs.BandTrackData>()
            //.ForMember(r => r.BandLong, opt => opt.MapFrom(src => src.BandID))
            //.ForMember(r => r.APLong, opt => opt.MapFrom(src => src.APID))
            //.ForMember(r => r.LastRSSI, opt => opt.MapFrom(src => src.LastRSSI))
            //   .ForMember(r => r.MaxRSSI, opt => opt.MapFrom(src => src.MaxRSSI))
            //.ForMember(r => r.StartTime, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.StartTime).ToLocalTime().DateTime))
            //.ForMember(r => r.EndTime, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.EndTime).ToLocalTime().DateTime))
            //    ;


            //    CreateMap<BandLib.Cloud.Models.BroadcastData, BandLib.Band>()
            //        .ForMember(r => r.Mac, opt => opt.MapFrom(src => src.mac.Trim()))
            //        .ForMember(r => r.Name, opt => opt.MapFrom(src => src.name))
            //        .ForMember(r => r.Model, opt => opt.MapFrom(src => src.name))
            //        .ForMember(r => r.CardNo, opt => opt.MapFrom(src => src.cardserno))
            //        .ForMember(r => r.CodeWrited, opt => opt.MapFrom(src => false))
            //        .ForMember(r => r.Connected, opt => opt.MapFrom(src => false))
            //        .ForMember(r => r.TimeSynced, opt => opt.MapFrom(src => false))
            //        .ForMember(r => r.MacWrited, opt => opt.MapFrom(src => false))
            //        .ForMember(r => r.LastTrySyncTime, opt => opt.MapFrom(src => BaseTimeHelper.GetLessThanMinDate(DateTime.MinValue)))
            //        .ForMember(r => r.LastConnectedTime, opt => opt.MapFrom(src => BaseTimeHelper.GetLessThanMinDate(DateTime.MinValue)))
            //        .ForMember(r => r.LastFindedTime, opt => opt.MapFrom(src => DateTime.Now))
            //        .ForMember(r => r.LastTryWriteCodeTime, opt => opt.MapFrom(src => BaseTimeHelper.GetLessThanMinDate(DateTime.MinValue)))
            //        .ForMember(r => r.LastTryWriteMacTime, opt => opt.MapFrom(src => BaseTimeHelper.GetLessThanMinDate(DateTime.MinValue)))
            //        .ForMember(r => r.BroadcastData, opt => opt.Ignore())
            //        ;


        }


    }
}
