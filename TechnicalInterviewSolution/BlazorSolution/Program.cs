using Backend.Interfaces;
using Backend.Models.Json;
using Backend.Objects;
using Backend.Objects.AnswerProviders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace BlazorSolution
{
    public class Program
    {
        /// <summary>
        /// The providers of answers.
        /// </summary>
        static List<IAnswerProvider<RegisteredPerson>> _answerProviders = new List<IAnswerProvider<RegisteredPerson>>()
        {
            new Over50AnswerProvider(),
            new MostRecentStillActiveAnswerProvider(),
            new FavoriteFruitsCountsAnswerProvider(),
            new MostCommenEyeColorAnswerProvider(),
            new TotalBalanceAnswerProvider(),
            new FullNameAnswerProvider()
        };

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<IAnswerAggregator<RegisteredPerson>, 
                            MultithreadedAnswerAggregator<RegisteredPerson>>(
                                _ => new MultithreadedAnswerAggregator<RegisteredPerson>(Program._answerProviders));
            builder.Services.AddSingleton<IAnswerRecordDataManager, AnswerRecordDataManager>(_ => new AnswerRecordDataManager(new AnswerContextFactory()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}