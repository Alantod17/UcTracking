using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using AwesomeDi.Api.Handlers.ResearchArticle;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AwesomeDi.UnitTest
{
	public class ResearchArticalTests
    {
        private _DbContext.AwesomeDiContext GetContext()
        {
            var options = new DbContextOptionsBuilder<_DbContext.AwesomeDiContext>()
                .UseSqlServer("Data Source=.;database=AwesomeDiRa3;trusted_connection=yes;")
                .Options;
            // Insert seed data into the database using one instance of the context
            return new _DbContext.AwesomeDiContext(options);

        }

        [Test]
        public void PaperList()
        {
            var iRIdList = new List<int>
            {
                1943, 1986, 2244, 2444, 2495, 2551, 2579, 2834, 2837, 2839, 2844, 3260, 3519, 3578, 3905, 3909, 4188,
                4428, 4792, 5124, 5377, 5453, 5482, 5500, 5501, 5506, 5507, 5689, 5933, 7476, 7864
            };
            var spectrumIdList = new List<int> { 2616, 2664, 2730, 2838, 3407, 3447, 3774, 3909, 4018, 4029, 5014, 5361, 5422, 5463, 5475, 5489, 5498, 5797, 5800, 5824, 8076 };
            var machineIdList = new List<int> { 2579,3454,3774,4061,4428,5422 };
            var replayList = new List<int> { 1828, 3067, 5462 };
            var debuggingProcess = new List<int> { 2876, 4848, 5442, 5447, 5467, 5491 };
            var reportClassification = new List<int> { 1933, 2138 };
            var excutionMonitoring = new List<int> { 1807, 1876, 1969, 1986, 1987, 2064, 2179, 2218, 2414, 2709, 3205, 5448, 5818 };
            using var context = GetContext();
            var raList = context.ResearchArticle.Where(x => x.IsDeleted == false && excutionMonitoring.Contains(x.Id)).ToList();
            var strList = new List<string>();
            foreach (var researchArticle in raList)
            {
                strList.Add("\\cite{" + researchArticle.Doi + "}");
            }

            var resStr = string.Join(",", strList);

        }

        [Test]
        public void CombineRefer()
        {
            var currentString = @"
            @inproceedings{kampmann2020,
  author    = {Alexander Kampmann and Nikolas Havrikov and Ezekiel Soremekun and Andreas Zeller},
  title     = {When does my Program do this? Learning Circumstances of Software Behavior},
  booktitle = {ACM Joint European Software Engineering Conference and Symposium on the Foundations of Software Engineering (ESEC/FSE)},
  year      = {2020},
  pages     = {1228–1239}
}

@inproceedings{10.1109/ICSME46990.2020.00063,
  author    = {Rahman, Mohammad Masudur and Khomh, Foutse and Castelluccio, Marco},
  booktitle = {2020 IEEE International Conference on Software Maintenance and Evolution (ICSME)},
  title     = {Why are Some Bugs Non-Reproducible? : –An Empirical Investigation using Data Fusion–},
  year      = {2020},
  volume    = {},
  number    = {},
  pages     = {605-616},
  doi       = {10.1109/ICSME46990.2020.00063}
}

@inproceedings{10.1145/1937117.1937125,
  author    = {LaToza, Thomas D. and Myers, Brad A.},
  title     = {Hard-to-Answer Questions about Code},
  year      = {2010},
  isbn      = {9781450305471},
  publisher = {Association for Computing Machinery},
  address   = {New York, NY, USA},
  url       = {https://doi.org/10.1145/1937117.1937125},
  doi       = {10.1145/1937117.1937125},
  abstract  = {To build new tools and programming languages that make it easier for professional
               software developers to create, debug, and understand code, it is helpful to better
               understand the questions that developers ask during coding activities. We surveyed
               professional software developers and asked them to list hard-to-answer questions that
               they had recently asked about code. 179 respondents reported 371 questions. We then
               clustered these questions into 21 categories and 94 distinct questions. The most frequently
               reported categories dealt with intent and rationale -- what does this code do, what
               is it intended to do, and why was it done this way? Many questions described very
               specific situations -- e.g, what does the code do when an error occurs, how to refactor
               without breaking callers, or the implications of a specific change on security. These
               questions revealed opportunities for both existing research tools to help developers
               and for developing new languages and tools that make answering these questions easier.},
  booktitle = {Evaluation and Usability of Programming Languages and Tools},
  articleno = {8},
  numpages  = {6},
  keywords  = {developer questions, program comprehension},
  location  = {Reno, Nevada},
  series    = {PLATEAU '10}
}

@inproceedings{10.1145/3338906.3338947,
  author    = {Chaparro, Oscar and Bernal-C\'{a}rdenas, Carlos and Lu, Jing and Moran, Kevin and Marcus, Andrian and Di Penta, Massimiliano and Poshyvanyk, Denys and Ng, Vincent},
  title     = {Assessing the Quality of the Steps to Reproduce in Bug Reports},
  year      = {2019},
  isbn      = {9781450355728},
  publisher = {Association for Computing Machinery},
  address   = {New York, NY, USA},
  url       = {https://doi.org/10.1145/3338906.3338947},
  doi       = {10.1145/3338906.3338947},
  abstract  = {A major problem with user-written bug reports, indicated by developers and documented
               by researchers, is the (lack of high) quality of the reported steps to reproduce the
               bugs. Low-quality steps to reproduce lead to excessive manual effort spent on bug
               triage and resolution. This paper proposes Euler, an approach that automatically identifies
               and assesses the quality of the steps to reproduce in a bug report, providing feedback
               to the reporters, which they can use to improve the bug report. The feedback provided
               by Euler was assessed by external evaluators and the results indicate that Euler correctly
               identified 98% of the existing steps to reproduce and 58% of the missing ones, while
               73% of its quality annotations are correct.},
  booktitle = {Proceedings of the 2019 27th ACM Joint Meeting on European Software Engineering Conference and Symposium on the Foundations of Software Engineering},
  pages     = {86–96},
  numpages  = {11},
  keywords  = {Textual Analysis, Dynamic Software Analysis, Bug Report Quality},
  location  = {Tallinn, Estonia},
  series    = {ESEC/FSE 2019}
}

@article{10.1109/TSE.2010.63,
  author  = {Zimmermann, Thomas and Premraj, Rahul and Bettenburg, Nicolas and Just, Sascha and Schroter, Adrian and Weiss, Cathrin},
  journal = {IEEE Transactions on Software Engineering},
  title   = {What Makes a Good Bug Report?},
  year    = {2010},
  volume  = {36},
  number  = {5},
  pages   = {618-643},
  doi     = {10.1109/TSE.2010.63}
}


@inproceedings{10.1145/3106237.3106285,
  author    = {Chaparro, Oscar and Lu, Jing and Zampetti, Fiorella and Moreno, Laura and Di Penta, Massimiliano and Marcus, Andrian and Bavota, Gabriele and Ng, Vincent},
  title     = {Detecting Missing Information in Bug Descriptions},
  year      = {2017},
  isbn      = {9781450351058},
  publisher = {Association for Computing Machinery},
  address   = {New York, NY, USA},
  url       = {https://doi.org/10.1145/3106237.3106285},
  doi       = {10.1145/3106237.3106285},
  abstract  = { Bug reports document unexpected software behaviors experienced by users. To be effective,
               they should allow bug triagers to easily understand and reproduce the potential reported
               bugs, by clearly describing the Observed Behavior (OB), the Steps to Reproduce (S2R),
               and the Expected Behavior (EB). Unfortunately, while considered extremely useful,
               reporters often miss such pieces of information in bug reports and, to date, there
               is no effective way to automatically check and enforce their presence. We manually
               analyzed nearly 3k bug reports to understand to what extent OB, EB, and S2R are reported
               in bug reports and what discourse patterns reporters use to describe such information.
               We found that (i) while most reports contain OB (i.e, 93.5%), only 35.2% and 51.4%
               explicitly describe EB and S2R, respectively; and (ii) reporters recurrently use 154
               discourse patterns to describe such content. Based on these findings, we designed
               and evaluated an automated approach to detect the absence (or presence) of EB and
               S2R in bug descriptions. With its best setting, our approach is able to detect missing
               EB (S2R) with 85.9% (69.2%) average precision and 93.2% (83%) average recall. Our
               approach intends to improve bug descriptions quality by alerting reporters about missing
               EB and S2R at reporting time. },
  booktitle = {Proceedings of the 2017 11th Joint Meeting on Foundations of Software Engineering},
  pages     = {396–407},
  numpages  = {12},
  keywords  = {Bug Descriptions Discourse, Automated Discourse Identification},
  location  = {Paderborn, Germany},
  series    = {ESEC/FSE 2017}
}


@inproceedings{10.1109/ICSE.2012.6227112,
  author    = {Zimmermann, Thomas and Nagappan, Nachiappan and Guo, Philip J. and Murphy, Brendan},
  booktitle = {2012 34th International Conference on Software Engineering (ICSE)},
  title     = {Characterizing and predicting which bugs get reopened},
  year      = {2012},
  volume    = {},
  number    = {},
  pages     = {1074-1083},
  doi       = {10.1109/ICSE.2012.6227112}
}

@inproceedings{10.1145/1958824.1958887,
  author    = {Guo, Philip J. and Zimmermann, Thomas and Nagappan, Nachiappan and Murphy, Brendan},
  title     = {Not My Bug! And Other Reasons for Software Bug Report Reassignments},
  year      = {2011},
  isbn      = {9781450305563},
  publisher = {Association for Computing Machinery},
  address   = {New York, NY, USA},
  url       = {https://doi.org/10.1145/1958824.1958887},
  doi       = {10.1145/1958824.1958887},
  abstract  = {Bug reporting/fixing is an important social part of the soft-ware development process.
               The bug-fixing process inher-ently has strong inter-personal dynamics at play, especially
               in how to find the optimal person to handle a bug report. Bug report reassignments,
               which are a common part of the bug-fixing process, have rarely been studied.In this
               paper, we present a large-scale quantitative and qualitative analysis of the bug reassignment
               process in the Microsoft Windows Vista operating system project. We quantify social
               interactions in terms of both useful and harmful reassignments. For instance, we found
               that reassignments are useful to determine the best person to fix a bug, contrary
               to the popular opinion that reassignments are always harmful. We categorized five
               primary reasons for reassignments: finding the root cause, determining ownership,
               poor bug report quality, hard to determine proper fix, and workload balancing. We
               then use these findings to make recommendations for the design of more socially-aware
               bug tracking systems that can overcome some of the inefficiencies we observed in our
               study.},
  booktitle = {Proceedings of the ACM 2011 Conference on Computer Supported Cooperative Work},
  pages     = {395–404},
  numpages  = {10},
  keywords  = {bug triaging, bug reassignment, bug tracking},
  location  = {Hangzhou, China},
  series    = {CSCW '11}
}



@inproceedings{10.1109/ICPC.2013.6613835,
  author    = {Roehm, Tobias and Gurbanova, Nigar and Bruegge, Bernd and Joubert, Christophe and Maalej, Walid},
  booktitle = {2013 21st International Conference on Program Comprehension (ICPC)},
  title     = {Monitoring user interactions for supporting failure reproduction},
  year      = {2013},
  volume    = {},
  number    = {},
  pages     = {73-82},
  doi       = {10.1109/ICPC.2013.6613835}
}

@article{10.1145/2659118.2659125,
  author     = {Agarwal, Pragya and Agrawal, Arun Prakash},
  title      = {Fault-Localization Techniques for Software Systems: A Literature Review},
  year       = {2014},
  issue_date = {September 2014},
  publisher  = {Association for Computing Machinery},
  address    = {New York, NY, USA},
  volume     = {39},
  number     = {5},
  issn       = {0163-5948},
  url        = {https://doi.org/10.1145/2659118.2659125},
  doi        = {10.1145/2659118.2659125}
}
@misc{unknown2012a,
  author = {National Science Foundation.},
  title  = {The Software-artifact Infrastructure Repository},
  url    = {http://sir.unl.edu/portal},
  year   = {2012}
}
@article{10.1016/j.infsof.2020.106295,
  title    = {Search-based fault localisation: A systematic mapping study},
  journal  = {Information and Software Technology},
  volume   = {123},
  pages    = {106295},
  year     = {2020},
  issn     = {0950-5849},
  doi      = {https://doi.org/10.1016/j.infsof.2020.106295},
  url      = {https://www.sciencedirect.com/science/article/pii/S0950584920300458},
  author   = {Plinio S. Leitao-Junior and Diogo M. Freitas and Silvia R. Vergilio and Celso G. Camilo-Junior and Rachel Harrison},
  keywords = {Meta-heuristic algorithms, Search-based fault localisation, Systematic mapping}
}
@article{10.1109/TSE.2016.2521368,
  author  = {Wong, W. Eric and Gao, Ruizhi and Li, Yihao and Abreu, Rui and Wotawa, Franz},
  journal = {IEEE Transactions on Software Engineering},
  title   = {A Survey on Software Fault Localization},
  year    = {2016},
  volume  = {42},
  number  = {8},
  pages   = {707-740},
  doi     = {10.1109/TSE.2016.2521368}
}
@article{budgen2008a,
  title     = {Using Mapping Studies in Software Engineering.},
  author    = {Budgen, David and Turner, Mark and Brereton, Pearl and Kitchenham, Barbara A},
  booktitle = {Ppig},
  volume    = {8},
  pages     = {195--204},
  year      = {2008}
}


@article{10.1016/j.infsof.2010.12.003,
  citation-number = {14},
  author          = {Mota Silveira Neto, P.A. and Carmo MacHado, I. and McGregor, J.D. and Almeida, E.S. and Lemos Meira, S.R.},
  title           = {A systematic mapping study of software product lines testing},
  date            = {2011-05},
  volume          = {53},
  pages           = {407–423,},
  doi             = {10.1016/j.infsof.2010.12.003.},
  language        = {en},
  journal         = {Information and Software Technology},
  number          = {5}
}
@article{10.1016/j.infsof.2015.03.007,
  citation-number = {15},
  author          = {Petersen, K. and Vakkalanka, S. and Kuzniarz, L.},
  title           = {Guidelines for conducting systematic mapping studies in software engineering: An update},
  volume          = {64},
  pages           = {1–18,},
  date            = {2015},
  url             = {doi:},
  doi             = {10.1016/j.infsof.2015.03.007.},
  language        = {en},
  journal         = {Information and Software Technology}
}
@inproceedings{petersen-a,
  author    = {Petersen, Kai and Feldt, Robert and Mujtaba, Shahid and Mattsson, Michael},
  title     = {Systematic Mapping Studies in Software Engineering},
  year      = {2008},
  abstract  = {BACKGROUND: A software engineering systematic map is a defined method to build a classification
               scheme and structure a software engineering field of interest. The analysis of results
               focuses on frequencies of publications for categories within the scheme. Thereby,
               the coverage of the research field can be determined. Different facets of the scheme
               can also be combined to answer more specific research questions.OBJECTIVE: We describe
               how to conduct a systematic mapping study in software engineering and provide guidelines.
               We also compare systematic maps and systematic reviews to clarify how to chose between
               them. This comparison leads to a set of guidelines for systematic maps.METHOD: We
               have defined a systematic mapping process and applied it to complete a systematic
               mapping study. Furthermore, we compare systematic maps with systematic reviews by
               systematically analyzing existing systematic reviews.RESULTS: We describe a process
               for software engineering systematic mapping studies and compare it to systematic reviews.
               Based on this, guidelines for conducting systematic maps are defined.CONCLUSIONS:
               Systematic maps and reviews are different in terms of goals, breadth, validity issues
               and implications. Thus, they should be used complementarily and require different
               methods (e.g, for analysis).},
  booktitle = {Proceedings of the 12th International Conference on Evaluation and Assessment in Software Engineering},
  pages     = {68–77},
  numpages  = {10},
  keywords  = {systematic mapping studies, systematic reviews, evidence based software engineering},
  location  = {Italy},
  series    = {EASE'08}
}

@misc{luis-a,
  title         = {Systematic Mapping Protocol: Have Systematic Reuse Benefits Been Transferred to Real-world Settings?},
  author        = {Jose Luis Barros-Justo and Fernando Pinciroli and Santiago Matalonga and Marco Aurelio Paz Gonzalez and Nelson Martinez Araujo},
  year          = {2016},
  eprint        = {1609.06553},
  archiveprefix = {arXiv},
  primaryclass  = {cs.SE}
}

@inproceedings{10.1145/2601248.2601254,
  author    = {Diebold, Philipp and Dahlem, Marc},
  title     = {Agile Practices in Practice: A Mapping Study},
  year      = {2014},
  isbn      = {9781450324762},
  publisher = {Association for Computing Machinery},
  address   = {New York, NY, USA},
  url       = {https://doi.org/10.1145/2601248.2601254},
  doi       = {10.1145/2601248.2601254},
  abstract  = {Background: Agile software development has been increasingly adopted during the last
               two decades. Nonetheless, many studies show that using agile methods as defined in
               the literature does not work very well. Thus, companies adapt these methods by just
               using parts of them (called agile practices). Objective: The goal of the literature
               study was to understand which agile practices are used in industry under different
               circumstances, such as different project types, domains, or processes. Method: We
               conducted a mapping study of empirical studies using agile practices in industry.
               The search strategy identified 1110 studies, of which 24 studies including 68 projects
               were analyzed. Results: The results of this study show that there are practices that
               are used more often and that the domain and the process also influence the application
               of different practices. Additionally, the findings confirm the assumption of Ken Schwaber
               that in most cases, agile methods are not used completely but that rather certain
               practices are adopted. Conclusions: Our results can be used by researchers to get
               a better idea of where and how to follow up research as well as by practitioners to
               get a better idea of which practices fit their needs and which are used by others.
               Therefore, our contribution increases the body of knowledge in agile practices usage.},
  booktitle = {Proceedings of the 18th International Conference on Evaluation and Assessment in Software Engineering},
  articleno = {30},
  numpages  = {10},
  keywords  = {systematic review, agile methods, mapping study, industrial usage, empirical SE, agile software development, agile practices},
  location  = {London, England, United Kingdom},
  series    = {EASE '14}
}

@article{10.1016/j.infsof.2015.03.007,
  citation-number = {19},
  author          = {Petersen, K. and Vakkalanka, S. and Kuzniarz, L.},
  title           = {Guidelines for conducting systematic mapping studies in software engineering: An update},
  volume          = {64},
  pages           = {1–18,},
  date            = {2015},
  doi             = {10.1016/j.infsof.2015.03.007.},
  language        = {en},
  journal         = {Information and Software Technology}
}
@inproceedings{10.1145/2771783.2771797,
  citation-number = {20},
  author          = {Wang, Q. and Parnin, C. and Orso, A.},
  title           = {Evaluating the Usefulness of IR-Based Fault Localization Techniques},
  date            = {2015},
  pages           = {1–11,},
  doi             = {10.1145/2771783.2771797.},
  language        = {en},
  booktitle       = {Proceedings of the 2015 International Symposium on Software Testing and Analysis}
}
@article{10.1145/2382756.2382784,
  citation-number = {21},
  author          = {Hofer, B. and Wotawa, F. and Abreu, R.},
  title           = {AI for the Win: Improving Spectrum-Based Fault Localization},
  volume          = {37},
  pages           = {1–8,},
  date            = {2012-11},
  doi             = {10.1145/2382756.2382784.},
  language        = {en},
  journal         = {SIGSOFT Softw. Eng. Notes},
  number          = {6}
}
@article{g2014a,
  citation-number = {22},
  author          = {Gómez, O.S. and Juristo, N. and Vegas, S.},
  title           = {Understanding replication of experiments in software engineering: A classification},
  volume          = {56},
  pages           = {1033–1048,},
  date            = {2014},
  language        = {en},
  journal         = {Information and software technology},
  number          = {8}
}
@book{runeson2012a,
  citation-number = {23},
  author          = {Runeson, P.},
  title           = {Case study research in software engineering: guidelines and examples, 1. Aufl},
  publisher       = {Wiley},
  date            = {2012},
  language        = {en},
  address         = {Hoboken, N.J}
}
@article{10.1007/978-1-84800-044-5,
  citation-number = {24},
  author          = {Shull, F. and Singer, J. and Sjøberg, D.I.K.},
  title           = {Guide to advanced empirical software engineering},
  pages           = {1–388,},
  date            = {2008},
  doi             = {10.1007/978-1-84800-044-5.},
  language        = {en},
  journal         = {Guide to Advanced Empirical Software Engineering}
}
@article{kitchenham2010a,
  citation-number = {25},
  author          = {Kitchenham, B.},
  title           = {Systematic literature reviews in software engineering – A tertiary study},
  volume          = {52},
  pages           = {792–805,},
  date            = {2010},
  language        = {en},
  journal         = {Information and software technology},
  number          = {8}
}
@inproceedings{10.1145/3196398.3196415,
  citation-number = {26},
  author          = {Rath, M. and Lo, D. and Mäder, P.},
  title           = {Analyzing Requirements and Traceability Information to Improve Bug Localization},
  date            = {2018},
  pages           = {442–453,},
  doi             = {10.1145/3196398.3196415.},
  language        = {en},
  booktitle       = {Proceedings of the 15th International Conference on Mining Software Repositories}
}
@article{10.1109/TR.2013.2285319,
  citation-number = {27},
  author          = {Wong, W.E. and Debroy, V. and Gao, R. and Li, Y.},
  title           = {The DStar Method for Effective Software Fault Localization},
  volume          = {63},
  pages           = {290–308,},
  date            = {2014},
  doi             = {10.1109/TR.2013.2285319.},
  language        = {en},
  journal         = {IEEE Transactions on Reliability},
  number          = {1}
}
@article{10.1109/ACCESS.2020.3025460,
  citation-number = {28},
  author          = {Cui, Z. and Jia, M. and Chen, X. and Zheng, L. and Liu, X.},
  title           = {Improving Software Fault Localization by Combining Spectrum and Mutation},
  volume          = {8},
  pages           = {172296–172307,},
  date            = {2020},
  doi             = {10.1109/ACCESS.2020.3025460.},
  language        = {en},
  journal         = {IEEE Access}
}
@inproceedings{10.1109/ICSTW.2011.66,
  citation-number = {29},
  author          = {Herbold, S. and Grabowski, J. and Waack, S. and Bünting, U.},
  title           = {Improved Bug Reporting and Reproduction through Non-intrusive GUI Usage Monitoring and Automated Replaying},
  date            = {2011},
  pages           = {232–241,},
  doi             = {10.1109/ICSTW.2011.66.},
  language        = {en},
  booktitle       = {2011 IEEE Fourth International Conference on Software Testing, Verification and Validation Workshops}
}
@article{10.1109/TSE.2014.2312918,
  citation-number = {30},
  author          = {Pei, Y. and Furia, C.A. and Nordio, M. and Wei, Y. and Meyer, B. and Zeller, A.},
  title           = {Automated Fixing of Programs with Contracts},
  volume          = {40},
  pages           = {427–449,},
  date            = {2014},
  doi             = {10.1109/TSE.2014.2312918.},
  language        = {en},
  journal         = {IEEE Transactions on Software Engineering},
  number          = {5}
}
@inproceedings{10.1145/2597073.2597098,
  citation-number = {31},
  author          = {Joorabchi, M.E. and Mirzaaghaei, M. and Mesbah, A.},
  title           = {Works for me! characterizing non-reproducible bug reports},
  date            = {2014-05},
  pages           = {62–71,},
  doi             = {10.1145/2597073.2597098.},
  language        = {en},
  booktitle       = {11th Working Conference on Mining Software Repositories, MSR 2014 - Proceedings}
}
@inproceedings{10.1109/ICSE.2013.6606613,
  citation-number = {32},
  author          = {Johnson, B. and Song, Y. and Murphy-Hill, E. and Bowdidge, R.},
  title           = {Why don’t software developers use static analysis tools to find bugs?},
  pages           = {672–681,},
  doi             = {10.1109/ICSE.2013.6606613.},
  language        = {en},
  booktitle       = {2013 35th International Conference on Software Engineering (ICSE), 2013}
}
@inproceedings{10.1109/ICPC.2015.14,
  citation-number = {33},
  author          = {White, M. and Linares-Vásquez, M. and Johnson, P. and Bernal-Cárdenas, C. and Poshyvanyk, D.},
  title           = {Generating Reproducible and Replayable Bug Reports from Android Application Crashes},
  date            = {2015-08},
  volume          = {2015-August},
  pages           = {48–59,},
  doi             = {10.1109/ICPC.2015.14.},
  language        = {en},
  booktitle       = {IEEE International Conference on Program Comprehension}
}
@inproceedings{10.1109/IWSM-MENSURA.2011.53,
  citation-number = {34},
  author          = {Sasaki, T. and Morisaki, S. and Matsumoto, K.},
  title           = {An Exploratory Study on the Impact of Usage of Screenshot in Software Inspection Recording Activity},
  date            = {2011},
  pages           = {251–256,},
  doi             = {10.1109/IWSM-MENSURA.2011.53.},
  language        = {en},
  booktitle       = {2011 Joint Conference of the 21st International Workshop on Software Measurement and the 6th International Conference on Software Process and Product Measurement}
}
@inproceedings{10.1145/2973839.2973853,
  citation-number = {35},
  author          = {Garnier, M. and Garcia, A.},
  title           = {On the Evaluation of Structured Information Retrieval-Based Bug Localization on 20 C\# Projects},
  date            = {2016},
  pages           = {123–132,},
  doi             = {10.1145/2973839.2973853.},
  language        = {en},
  booktitle       = {Proceedings of the 30th Brazilian Symposium on Software Engineering}
}
@article{10.1007/s10664-016-9492-y,
  author  = {Kuhrmann, Marco
             and Fern{\'a}ndez, Daniel M{\'e}ndez
             and Daneva, Maya},
  title   = {On the pragmatic design of literature studies in software engineering: an experience-based guideline},
  journal = {Empirical Software Engineering},
  year    = {2017},
  month   = {Dec},
  day     = {01},
  volume  = {22},
  number  = {6},
  pages   = {2852-2891},
  issn    = {1573-7616},
  doi     = {10.1007/s10664-016-9492-y},
  url     = {https://doi.org/10.1007/s10664-016-9492-y}
}

@article{10.1145/3437479.3437483,
  author     = {Ralph, Paul},
  title      = {ACM SIGSOFT Empirical Standards Released},
  year       = {2021},
  issue_date = {January 2021},
  publisher  = {Association for Computing Machinery},
  address    = {New York, NY, USA},
  volume     = {46},
  number     = {1},
  issn       = {0163-5948},
  url        = {https://doi.org/10.1145/3437479.3437483},
  doi        = {10.1145/3437479.3437483},
  abstract   = {In October 2020, The ACM SIGSOFT Paper and Peer Review Quality Task Force released
                its first empirical standards. An empirical standard is a brief public document that
                communicates expectations for a specific kind of study (e.g. a questionnaire survey)
                [1]. (All quotations below are from the Empirical Standards report [1] unless otherwise
                noted.)},
  journal    = {SIGSOFT Softw. Eng. Notes},
  month      = jan,
  pages      = {19},
  numpages   = {1}
}

@inproceedings{10.1145/2786805.2786880,
  author    = {Le, Tien-Duy B. and Oentaryo, Richard J. and Lo, David},
  title     = {Information Retrieval and Spectrum Based Bug Localization: Better Together},
  year      = {2015},
  isbn      = {9781450336758},
  publisher = {Association for Computing Machinery},
  address   = {New York, NY, USA},
  url       = {https://doi.org/10.1145/2786805.2786880},
  doi       = {10.1145/2786805.2786880},
  abstract  = { Debugging often takes much effort and resources. To help developers debug, numerous
               information retrieval (IR)-based and spectrum-based bug localization techniques have
               been proposed. IR-based techniques process textual information in bug reports, while
               spectrum-based techniques process program spectra (i.e, a record of which program
               elements are executed for each test case). Both eventually generate a ranked list
               of program elements that are likely to contain the bug. However, these techniques
               only consider one source of information, either bug reports or program spectra, which
               is not optimal. To deal with the limitation of existing techniques, in this work,
               we propose a new multi-modal technique that considers both bug reports and program
               spectra to localize bugs. Our approach adaptively creates a bug-specific model to
               map a particular bug to its possible location, and introduces a novel idea of suspicious
               words that are highly associated to a bug. We evaluate our approach on 157 real bugs
               from four software systems, and compare it with a state-of-the-art IR-based bug localization
               method, a state-of-the-art spectrum-based bug localization method, and three state-of-the-art
               multi-modal feature location methods that are adapted for bug localization. Experiments
               show that our approach can outperform the baselines by at least 47.62%, 31.48%, 27.78%,
               and 28.80% in terms of number of bugs successfully localized when a developer inspects
               1, 5, and 10 program elements (i.e, Top 1, Top 5, and Top 10), and Mean Average Precision
               (MAP) respectively. },
  booktitle = {Proceedings of the 2015 10th Joint Meeting on Foundations of Software Engineering},
  pages     = {579–590},
  numpages  = {12},
  keywords  = {Information Retrieval, Program Spectra, Bug Localization},
  location  = {Bergamo, Italy},
  series    = {ESEC/FSE 2015}
}

@inproceedings{10.1109/COMPSACW.2011.92,
  author    = {Debroy, Vidroha and Wong, W. Eric},
  booktitle = {2011 IEEE 35th Annual Computer Software and Applications Conference Workshops},
  title     = {On the Consensus-Based Application of Fault Localization Techniques},
  year      = {2011},
  volume    = {},
  number    = {},
  pages     = {506-511},
  doi       = {10.1109/COMPSACW.2011.92}
}

@inproceedings{10.1109/ICST.2013.24,
  author    = {Liu, Chen and Yang, Jinqiu and Tan, Lin and Hafiz, Munawar},
  booktitle = {2013 IEEE Sixth International Conference on Software Testing, Verification and Validation},
  title     = {R2Fix: Automatically Generating Bug Fixes from Bug Reports},
  year      = {2013},
  volume    = {},
  number    = {},
  pages     = {282-291},
  doi       = {10.1109/ICST.2013.24}
}

@inproceedings{10.5555/2819419.2819421,
  author    = {Wang, Nan and Zheng, Zheng and Zhang, Zhenyu and Chen, Cheng},
  title     = {FLAVS: A Fault Localization Add-in for Visual Studio},
  year      = {2015},
  publisher = {IEEE Press},
  abstract  = {Dynamic fault localization is a representative concept and product proposed by academia
               to alleviate software engineering pains, but it is rarely heard adopted or used in
               realistic development. Realizing the difficulties in transferring the approaches of
               dynamic fault localization to practical tools, this paper gives our work FLAVS, whose
               add-in implementation organically and seamlessly integrates the approach of dynamic
               fault localization with software IDEs. The tool is useful for developers using Microsoft
               Visual Studio platform to debug and test programs with complex bugs. Besides, it is
               also valuable for researchers to design new fault localization methods and draw performance
               comparison among different method candidates.},
  booktitle = {Proceedings of the First International Workshop on Complex FaUlts and Failures in LargE Software Systems},
  pages     = {1–6},
  numpages  = {6},
  keywords  = {fault localization tools, fault localization, microsoft visual studio add-in},
  location  = {Florence, Italy},
  series    = {COUFLESS '15}
}

@inproceedings{10.1145/2666581.2666593,
  author    = {Li, Heng and Liu, Yuzhen and Zhang, Zhenyu and Liu, Jian},
  title     = {Program Structure Aware Fault Localization},
  year      = {2014},
  isbn      = {9781450332262},
  publisher = {Association for Computing Machinery},
  address   = {New York, NY, USA},
  url       = {https://doi-org.ezproxy.canterbury.ac.nz/10.1145/2666581.2666593},
  doi       = {10.1145/2666581.2666593},
  abstract  = { Software testing is always an effective method to show the presence of bugs in programs,
               while debugging is never an easy task to remove a bug from a program. To facilitate
               the debugging task, statistical fault localization estimates the location of faults
               in programs automatically by analyzing the program executions to narrow down the suspicious
               region. We observe that program structure has strong impacts on the assessed suspiciousness
               of program elements. However, existing techniques inadequately pay attention to this
               problem. In this paper, we emphasize the biases caused by program structure in fault
               localization, and propose a method to address them. Our method is dedicated to boost
               a fault localization technique by adapting it to various program structures, in a
               software development process. It collects the suspiciousness of program elements when
               locating historical faults, statistically captures the biases caused by program structure,
               and removes such an impact factor from a fault localization result. An empirical study
               using the Siemens test suite shows that our method can greatly improve the effectiveness
               of the most representative fault localization Tarantula. },
  booktitle = {Proceedings of the International Workshop on Innovative Software Development Methodologies and Practices},
  pages     = {40–48},
  numpages  = {9},
  keywords  = {Software testing, fault localization, program structure},
  location  = {Hong Kong, China},
  series    = {InnoSWDev 2014}
}

@article{10.1109/TDSC.2004.2,
  author  = {Avizienis, A. and Laprie, J.-C. and Randell, B. and Landwehr, C.},
  journal = {IEEE Transactions on Dependable and Secure Computing},
  title   = {Basic concepts and taxonomy of dependable and secure computing},
  year    = {2004},
  volume  = {1},
  number  = {1},
  pages   = {11-33},
  doi     = {10.1109/TDSC.2004.2}
}

@inproceedings{10.1109/ISSREW.2014.36,
  author    = {Siegmund, Benjamin and Perscheid, Michael and Taeumel, Marcel and Hirschfeld, Robert},
  booktitle = {2014 IEEE International Symposium on Software Reliability Engineering Workshops},
  title     = {Studying the Advancement in Debugging Practice of Professional Software Developers},
  year      = {2014},
  volume    = {},
  number    = {},
  pages     = {269-274},
  doi       = {10.1109/ISSREW.2014.36}
}


@inproceedings{10.1109/COMAPP.2018.8460381,
  author    = {Zayour, Iyad and Mavromoustakis, Constandinos and El-Hajj-Diab, Bilal and Rahil, Ahmad},
  booktitle = {2018 International Conference on Computer and Applications (ICCA)},
  title     = {Towards an Understanding of the Causes of Difficulties in Debugging},
  year      = {2018},
  volume    = {},
  number    = {},
  pages     = {383-389},
  doi       = {10.1109/COMAPP.2018.8460381}
}
            ".ToLower();

            using var context = GetContext();
            var raList = context.ResearchArticle.Where(x => x.IsDeleted == false).ToList();
            var refStrList = new List<string>();
            var count = 0;
            foreach (var researchArticle in raList)
            {
                var refLines = researchArticle.DocumentFilePath.Split("\n\t").ToList();
                var keyLine = refLines.First();
                var doi = keyLine.Substring(keyLine.IndexOf("{") + 1).Replace(",","");
                researchArticle.Doi = doi;
                if(!currentString.Contains(doi.ToLower())){
                    refStrList.Add(researchArticle.DocumentFilePath);
                    count++;
                }
            }

            context.SaveChanges();
            var resStr = string.Join("\n\t\n\t", refStrList);

        }

        [Test]
        public void MoveDoiToKey()
        {

            using var context = GetContext();
            var raList = context.ResearchArticle.Where(x => x.IsDeleted == false).ToList();
            foreach (var researchArticle in raList)
            {
                var refLines = researchArticle.DocumentFilePath.Split("\n\t").ToList();
                var keyLine = refLines.First();
                var doi = refLines.FirstOrDefault(x => x.Contains("doi = {"))?.Replace("doi = {","")?.Replace("},","");
                if (!string.IsNullOrWhiteSpace(doi))
                {
                    var newKeyLine = keyLine.Substring(0, keyLine.IndexOf("{") + 1)+doi+",";
                    researchArticle.DocumentFilePath = researchArticle.DocumentFilePath.Replace(keyLine, newKeyLine);
                    context.SaveChanges();

                }
            }

        }

        [Test]
        public async Task GetRef()
        {
            using var context = GetContext();
            var raList = context.ResearchArticle.Where(x => x.IsDeleted == false && x.DocumentFilePath == "Invalid DOI").ToList();
            var httpClient = new HttpClient();
            foreach (var researchArticle in raList)
            {
                var response = await httpClient.GetAsync($"https://doi2bib.org/2/doi2bib?id={researchArticle.Doi}");
                var text = await response.Content.ReadAsStringAsync();
                researchArticle.DocumentFilePath = text;
            }

            context.SaveChanges();
        }

        [Test]
        public void Test()
        {
            using var context = GetContext();
            var handle = new GetResearchArticleInfoListStatics(context);
            var res = handle.Handle();
            var resJson = JsonConvert.SerializeObject(res);
        }

        private GetResearchArticleInfoListStaticsResult GetResultData()
        {
            var options = new DbContextOptionsBuilder<_DbContext.AwesomeDiContext>()
                .UseSqlServer("Data Source=.;database=AwesomeDiRa3;trusted_connection=yes;")
                .Options;
            // Insert seed data into the database using one instance of the context
            using var context = new _DbContext.AwesomeDiContext(options);
            var handle = new GetResearchArticleInfoListStatics(context);
            return handle.Handle();
        }

        [Test]
        public void researchMethodList_researchProblemList()
        {
            using var context = GetContext();
            var handle = new GetResearchArticleInfoListStatics(context);
            var res = handle.Handle();
            var researchMethod = res.ResearchMethodList.Select(x => x.TagName).ToList();
            var researchedProblemType = res.ResearchedProblemTypeList.Select(x => x.TagName).ToList();
            var researchContext = res.ResearchContextList.Select(x => x.TagName).ToList();
            var researchDataSource = res.ResearchDataSourceList.Select(x => x.TagName).ToList();
            var researchPlatform = res.ResearchPlatformList.Select(x => x.TagName).ToList();
            var researchedTechnology = res.ResearchedTechnologyList.Select(x => x.TagName).ToList();
            var researchedApproach = res.ResearchedApproachList.Select(x => x.TagName).ToList();
            GenerateList(researchedProblemType, researchMethod, "researchedProblemType", "researchMethod", context);
            GenerateList(researchedProblemType, researchContext, "researchedProblemType", "researchContext", context);
            GenerateList(researchedProblemType, researchDataSource, "researchedProblemType", "researchDataSource", context);
            GenerateList(researchedProblemType, researchPlatform, "researchedProblemType", "researchPlatform", context);
            GenerateList(researchedProblemType, researchedTechnology, "researchedProblemType", "researchedTechnology", context);
            
            
            GenerateList(researchedApproach, researchMethod, "researchedApproach", "researchMethod", context);
            GenerateList(researchedApproach, researchContext, "researchedApproach", "researchContext", context);
            GenerateList(researchedApproach, researchDataSource, "researchedApproach", "researchDataSource", context);
            GenerateList(researchedApproach, researchPlatform, "researchedApproach", "researchPlatform", context);
            GenerateList(researchedApproach, researchedTechnology, "researchedApproach", "researchedTechnology", context);
            
            GenerateList(researchContext, researchedTechnology, "researchContext", "researchedTechnology", context);
        }

        private static void GenerateList(List<string> tag1List, List<string> tag2List,string tag1Name, string tag2Name, _DbContext.AwesomeDiContext context)
        {
            var resList = new List<Data>();
            for (int i = 0; i < tag1List.Count; i++)
            {
                var tag1 = tag1List[i];
                for (int j = 0; j < tag2List.Count; j++)
                {
                    var tag2 = tag2List[j];
                    var data = new Data
                    {
                        Tag1Name = tag1,
                        Tag1Value = (i + 1) * 10,
                        Tag2Name = tag2,
                        Tag2Value = (j + 1) * 1000,
                        Count = context.ResearchArticle.Count($"IsDeleted == false && {tag1Name}.Contains(@0) && {tag2Name}.Contains(@1)", tag1, tag2)
                    };
                    resList.Add(data);
                }
            }

            // var fileByte = HelperExcel.GetExcelSpreadsheetByteArrayXlsxFromList(resList);
            // File.WriteAllBytes(@$"C:\temp\{tag1Name}_{tag2Name}_{DateTime.Now.Ticks}.xlsx", fileByte);
        }

        private class Data
        {
            public string Tag1Name { get; set; }
            public int Tag1Value { get; set; }
            public string Tag2Name { get; set; }
            public int Tag2Value { get; set; }
            public int Count { get; set; }
        }
    }
}