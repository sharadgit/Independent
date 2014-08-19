using DGuide.Infrastructure.Core;
using DGuide.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace DGuide.Infrastructure
{
    public class DGuideInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DGuideContext>
    //public class DGuideInitializer : System.Data.Entity.DropCreateDatabaseAlways<DGuideContext>
    {
        protected override void Seed(DGuideContext context)
        {
            var versions = new List<DbVersion>
            {
                new DbVersion { Version = "10.02.0" },
                new DbVersion { Version = "10.01.0" },
                new DbVersion { Version = "10.00.0" }
            };
            versions.ForEach(v => context.DbVersions.Add(v));
            context.SaveChanges();

            var itemTags = new List<ItemTag>
            {
                new ItemTag { Tag = "C#" },
                new ItemTag { Tag = "SQL" },
                new ItemTag { Tag = "EDI" }
            };

            var articles = new List<Article>
            {
                new Article {
                    Author          = "rbobby", 
                    Title           = "Create a New Article", 
                    Description     = "Adding an article is a two step process. First step creates an article entry, and second step creates one or more steps. Proper authorization is needed to create an article. Please contact your system administrator if you do not have one already.", 
                    DisplayStatus   = DisplayStatus.Normal,
                    TimeStamp       = new DateTime(2014, 1, 1)
                    },
                new Article {
                    Author          = "rbobby",
                    Title           = "Editing an Existing Article", 
                    Description     = "Editing process is broken down by sections. Each section in the article needs to be edited individually. Proper authorization is needed to edit an article.", 
                    DisplayStatus   = DisplayStatus.Normal,
                    TimeStamp       = new DateTime(2011, 1, 1)
                    },
                new Article {
                    Author          = "rbobby", 
                    Title           = "Ask a New Question", 
                    Description     = "Asking a new question requires proper authorization. Please contact your system administrator if you do not have an account.", 
                    DisplayStatus   = DisplayStatus.Normal,
                    TimeStamp       = new DateTime(2011, 1, 1)
                    },
                new Article {
                    Author          = "rbobby", 
                    Title           = "Answer an Existing Question", 
                    Description     = "Answering a question requires proper authorization.",
                    DisplayStatus   = DisplayStatus.Normal,
                    TimeStamp       = new DateTime(2011, 1, 1)
                    }
            };
            articles.ForEach(a => context.Articles.Add(a));
            context.SaveChanges();

            var sections = new List<Section> 
            {
                new Section { 
                    ArticleId     = 1,
                    Sequence      = 1, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Add Article Title and Description", 
                    Content       = "Article title and description are both required to add a new article. Title should be concise and clear. It should summarize the reason for creating this article. Description is additional description to supplement the title. Title and description are visible in article list."
                },
                new Section { 
                    ArticleId     = 1,
                    Sequence      = 2, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Add Section(s)", 
                    Content       = "Add one or more sections as necessary to explain the article. An optional header can be added for the section. Main body of the section is stored in Content field. The Format property determines if content will be displayed as is or rendered as HTML. HTML allows greater flexibility for user who require more control over section formatting."
                },
                new Section { 
                    ArticleId     = 1,
                    Sequence      = 3, 
                    ContentFormat = DataFormat.HTML,
                    Header        = "C# Code Example", 
                    Content       = @"<pre style=""font-family:Consolas;font-size:13;color:black;background:white;""><span style=""color:blue;"">public</span>&nbsp;<span style=""color:blue;"">class</span>&nbsp;<span style=""color:#2b91af;"">OrgVersion</span>
                                    {
                                    &nbsp;&nbsp;&nbsp;&nbsp;[<span style=""color:#2b91af;"">Key</span>]
                                    &nbsp;&nbsp;&nbsp;&nbsp;[<span style=""color:#2b91af;"">DatabaseGeneratedAttribute</span>(<span style=""color:#2b91af;"">DatabaseGeneratedOption</span>.Identity)]
                                    &nbsp;&nbsp;&nbsp;&nbsp;<span style=""color:blue;"">public</span>&nbsp;<span style=""color:blue;"">int</span>&nbsp;Id&nbsp;{&nbsp;<span style=""color:blue;"">get</span>;&nbsp;<span style=""color:blue;"">set</span>;&nbsp;}
 
                                    &nbsp;&nbsp;&nbsp;&nbsp;[<span style=""color:#2b91af;"">Required</span>]
                                    &nbsp;&nbsp;&nbsp;&nbsp;<span style=""color:blue;"">public</span>&nbsp;<span style=""color:blue;"">string</span>&nbsp;Version&nbsp;{&nbsp;<span style=""color:blue;"">get</span>;&nbsp;<span style=""color:blue;"">set</span>;&nbsp;}
                                    }</pre>"
                },
                new Section { 
                    ArticleId     = 1,
                    Sequence      = 4, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "SQL Code Example", 
                    Content       = "Please edit this field with proper HTML tags."
                },
                new Section { 
                    ArticleId     = 2,
                    Sequence      = 1, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Editing Article", 
                    Content       = "Link to edit article properties is in the details page. This link will open the article editor page."
                },
                new Section { 
                    ArticleId     = 2,
                    Sequence      = 2, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Editing Section(s)", 
                    Content       = "Link to edit section is underneath each section in Article details page. The link will open section editor page."
                },
                new Section { 
                    ArticleId     = 2,
                    Sequence      = 3, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Article Display Status", 
                    Content       = @"Article display status (defaulted to Normal) controls visibility to end users. Setting this value to Hidden will hide the article in article list and searches. ""Editing"" display status is meant to notify other users that you are currently editing the article. This status is for notification only; concurrency checks are not enforced. Please feel free to implement this feature if you want to contribute."
                },
                new Section { 
                    ArticleId     = 2,
                    Sequence      = 4, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Version", 
                    Content       = "Version is used to mark articles that apply to a specific release of a product or database. Enhancements to a product may require a new version of the article."
                },
                new Section { 
                    ArticleId     = 3,
                    Sequence      = 1, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Question Header", 
                    Content       = "Question header is a concise and clear summary of the issue that requires an explanation. Header is displayed in Questions list, and used for Searches."
                },
                new Section { 
                    ArticleId     = 3,
                    Sequence      = 2, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Question Description", 
                    Content       = "Question description is an elaborate description of the issue. Be very descriptive in the description because it will help fellow contributors answer your question promptly."
                },
                new Section { 
                    ArticleId     = 3,
                    Sequence      = 3, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Text Format", 
                    Content       = "Text format is sufficient for simpler questions that can be explained in written language. However, it cannot format source code nicely."
                },
                new Section { 
                    ArticleId     = 3,
                    Sequence      = 4, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "HTML Format", 
                    Content       = "HTML format is more powerful but requires HTML tags. This is the preferred method to include source code in your question."
                },
                new Section { 
                    ArticleId     = 4,
                    Sequence      = 1, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Answer Content", 
                    Content       = "Any authorized user may answer an existing question. Please use discretion when you answer questions. Please be courteous and provide reference to enforce your answer. There will be more than one way to solve a problem (specially with programming questions), we encourage you to answer the question if you have a different solution."
                },
                new Section { 
                    ArticleId     = 4,
                    Sequence      = 2, 
                    ContentFormat = DataFormat.TEXT,
                    Header        = "Text Format", 
                    Content       = "Text format is sufficient for simpler answers that can be explained in written language. However, it cannot format source code nicely."
                },
                new Section { 
                    ArticleId     = 4,
                    Sequence      = 3, 
                    Header        = "HTML Format", 
                    ContentFormat = DataFormat.HTML,
                    Content       = "HTML format is more powerful but requires HTML tags. This is the preferred method to include source code in your question."
                }
            };
            sections.ForEach(s => context.Sections.Add(s));
            context.SaveChanges();

            var questions = new List<Question>
            {
                new Question {
                    Header        = "How do I make coffee?",
                    ContentFormat = DataFormat.TEXT,
                    Content       = "I would like to help out my fellow developers by making coffee, but I do not know the proper method. I'm afraid I'll make to too strong or too weak. How do I turn off the coffee machine after I brew a pot? Thanks",
                    DisplayStatus = DisplayStatus.Normal,
                    Author        = "new developer",
                    TimeStamp     = new DateTime(2014,1,1)
                },
                new Question {
                    Header        = "Need to embed C# source code in my question description",
                    ContentFormat = DataFormat.TEXT,
                    Content       = "I have a question about my C# code. I want to include the source code in the description but I cannot format it nicely. I tried both Text and HTML format to no avail. Please help!",
                    DisplayStatus = DisplayStatus.Normal,
                    Author        = "new developer",
                    TimeStamp     = new DateTime(2014,1,1)
                }
            };
            questions.ForEach(q => context.Questions.Add(q));
            context.SaveChanges();
            
            var answers = new List<Answer>
            {
                new Answer {
                    QuestionId    = 1,
                    Content       = "First, this is a great question. I recommend 5-6 scoops for medium, and 6-7 for stronger brew. Do not turn off the coffee machine after you brew a pot. The machine needs to be ON to keep the water hot. People who drink tea need hot water. Therefore, please leave the machine turned on.",
                    ContentFormat = DataFormat.TEXT,
                    Author        = "rbobby",
                    TimeStamp     = new DateTime(2014,1,11)
                },
                new Answer {
                    QuestionId    = 1,
                    Content       = "Last person to leave the office, please make sure that the coffee machine is turned off.",
                    ContentFormat = DataFormat.TEXT,
                    Author        = "mbolton",
                    TimeStamp     = new DateTime(2014,1,22)
                },
                new Answer {
                    QuestionId    = 2,
                    Content       = "In order to display source code in nice format you need to include your code in <pre> and <code> tags and apply styles for formatting. Achieving this manually is a tedious process. If you are using Visual Studio, you can install Productivity Power Tools. This tool has Copy Html Markup functionality. Select the code, copy Html markup, and paste the Html in Content field.",
                    ContentFormat = DataFormat.TEXT,
                    Author        = "mbolton",
                    TimeStamp     = new DateTime(2014,1,22)
                },
                new Answer {
                    QuestionId    = 2,
                    Content       = "For TSQL code I use SQL Copy Tool.",
                    ContentFormat = DataFormat.TEXT,
                    Author        = "rbobby",
                    TimeStamp     = new DateTime(2014,1,22)
                }
            };
            answers.ForEach(a => context.Answers.Add(a));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
