using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Service
{
    public class SharesiesInstrumentsPriceHistoryResult
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
    public class SharesiesInstrumentsResult
    {
        public int? Total { get; set; }
        public int? CurrentPage { get; set; }
        public int? ResultsPerPage { get; set; }
        public int? NumberOfPages { get; set; }
        public List<SharesiesInstruments> Instruments { get; set; }
    }
    public class SharesiesInstruments
    {
        public string Id { get; set; }
        public string InstrumentType { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public List<string> Categories { get; set; }
        public int RiskRating { get; set; }
        public string ExchangeCountry { get; set; }
        public string UrlSlug { get; set; }
        public SharesiesInstrumentComparisonPrice ComparisonPrices { get; set; }
        public decimal MarketPrice { get; set; }
        public DateTime? MarketLastCheck { get; set; }
    }

    public class SharesiesInstrumentComparisonPrice
    {
        [JsonProperty("1d")]
        public SharesiesInstrumentComparisonPriceDetail Day { get; set; }
        [JsonProperty("1w")]
        public SharesiesInstrumentComparisonPriceDetail Week { get; set; }
        [JsonProperty("1m")]
        public SharesiesInstrumentComparisonPriceDetail Month { get; set; }
        [JsonProperty("3m")]
        public SharesiesInstrumentComparisonPriceDetail ThreeMonth { get; set; }
        [JsonProperty("6m")]
        public SharesiesInstrumentComparisonPriceDetail SixMonth { get; set; }
        [JsonProperty("1y")]
        public SharesiesInstrumentComparisonPriceDetail Year { get; set; }
        [JsonProperty("5y")]
        public SharesiesInstrumentComparisonPriceDetail FiveYear { get; set; }
    }
    public class SharesiesInstrumentComparisonPriceDetail
    {
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal Percent { get; set; }
        public decimal Max { get; set; }
        public decimal Min { get; set; }
    }


    public class LoginResult
    {
        public bool authenticated { get; set; }
        public bool can_enter_address_token { get; set; }
        public Can_Write_Until can_write_until { get; set; }
        public string distill_token { get; set; }
        public List<string> flags { get; set; }
        public string ga_id { get; set; }
        public bool include_sold_investments { get; set; }
        public Live_Data live_data { get; set; }
        public bool nzx_is_open { get; set; }
        public Nzx_Next_Open nzx_next_open { get; set; }
        public List<Portfolio> portfolio { get; set; }
        public string portfolio_filter_preference { get; set; }
        public string portfolio_sort_preference { get; set; }
        public string rakaia_token { get; set; }
        public Rakaia_Token_Expiry rakaia_token_expiry { get; set; }
        public string referral_code { get; set; }
        public string type { get; set; }
        public User user { get; set; }
        public List<User_List> user_list { get; set; }
    }

    public class Can_Write_Until
    {
        public long quantum { get; set; }
    }

    public class Live_Data
    {
        public bool eligible_for_free_month { get; set; }
        public bool is_active { get; set; }
    }

    public class Nzx_Next_Open
    {
        public long quantum { get; set; }
    }

    public class Rakaia_Token_Expiry
    {
        public long quantum { get; set; }
    }

    public class User
    {
        public bool account_frozen { get; set; }
        public string account_reference { get; set; }
        public bool account_restricted { get; set; }
        public object account_restricted_date { get; set; }
        public Address address { get; set; }
        public object address_reject_reason { get; set; }
        public string address_state { get; set; }
        public Checks checks { get; set; }
        public Competition_Entries competition_entries { get; set; }
        public string email { get; set; }
        public int first_tax_year { get; set; }
        public Has_Seen has_seen { get; set; }
        public string holding_balance { get; set; }
        public string home_currency { get; set; }
        public string id { get; set; }
        public string id_type { get; set; }
        public string intercom { get; set; }
        public string ird_number { get; set; }
        public bool is_dependent { get; set; }
        public bool is_owner_prescribed { get; set; }
        public string jurisdiction { get; set; }
        public string maximum_withdrawal_amount { get; set; }
        public bool mfa_enabled { get; set; }
        public string minimum_wallet_balance { get; set; }
        public object other_prescribed_participant { get; set; }
        public object[] participant_emails { get; set; }
        public string phone { get; set; }
        public object pir { get; set; }
        public string portfolio_id { get; set; }
        public Portfolio_Intro_Cards portfolio_intro_cards { get; set; }
        public string preferred_name { get; set; }
        public bool prescribed_approved { get; set; }
        public object prescribed_participant { get; set; }
        public string[] recent_searches { get; set; }
        public bool seen_first_time_autoinvest { get; set; }
        public bool seen_first_time_investor { get; set; }
        public string state { get; set; }
        public object[] tax_residencies { get; set; }
        public int tax_year { get; set; }
        public object tfn_number { get; set; }
        public object transfer_age { get; set; }
        public bool transfer_age_passed { get; set; }
        public bool us_equities_enabled { get; set; }
        public string us_tax_treaty_status { get; set; }
        public Wallet_Balances wallet_balances { get; set; }
        public string[] watchlist { get; set; }
    }

    public class Address
    {
        public Components components { get; set; }
        public string formatted { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Components
    {
        public string administrative_area { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string postal_code { get; set; }
        public string route { get; set; }
        public string street_number { get; set; }
    }

    public class Checks
    {
        public bool address_entered { get; set; }
        public bool address_verified { get; set; }
        public bool dependent_declaration { get; set; }
        public bool id_verified { get; set; }
        public Identity_Verification identity_verification { get; set; }
        public object latest_identity_verification_check { get; set; }
        public bool made_cumulative_deposits_over_one_thousand { get; set; }
        public bool made_cumulative_deposits_over_threshold { get; set; }
        public bool made_deposit { get; set; }
        public bool nature_and_purpose_answered { get; set; }
        public bool prescribed_answered { get; set; }
        public bool tax_questions { get; set; }
        public bool tc_accepted { get; set; }
    }

    public class Identity_Verification
    {
        public bool additional_verification_required { get; set; }
        public object additional_verification_required_reason { get; set; }
        public bool address_entered { get; set; }
        public bool address_verified { get; set; }
        public bool bank_name_match_verified { get; set; }
        public bool biometric_verified { get; set; }
        public bool id_verified { get; set; }
        public bool is_identity_linked { get; set; }
        public object latest_biometric_verification_check { get; set; }
        public bool manual_id_verified { get; set; }
        public bool name_and_dob_verified { get; set; }
        public string primary_id_type { get; set; }
        public bool secondary_id_verified { get; set; }
        public object secondary_identity_document_check { get; set; }
    }

    public class Competition_Entries
    {
        public Spring_Competition_2021 spring_competition_2021 { get; set; }
    }

    public class Spring_Competition_2021
    {
        public Eligible_Transactions[] eligible_transactions { get; set; }
        public int entries { get; set; }
    }

    public class Eligible_Transactions
    {
        public string amount { get; set; }
        public string balance { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public int line_number { get; set; }
        public string memo { get; set; }
        public string reason { get; set; }
        public Timestamp timestamp { get; set; }
        public int transaction_id { get; set; }
    }

    public class Timestamp
    {
        public long quantum { get; set; }
    }

    public class Has_Seen
    {
        public bool au_shares_intro { get; set; }
        public bool autoinvest { get; set; }
        public bool companies { get; set; }
        public bool completed_topup_goal_tile { get; set; }
        public bool create_topup_goal_tile { get; set; }
        public bool exchange_investor { get; set; }
        public bool funds { get; set; }
        public bool investor { get; set; }
        public bool limit_orders { get; set; }
        public bool managed_funds_investor { get; set; }
        public bool show_au_currency { get; set; }
        public bool welcome_screens { get; set; }
    }

    public class Portfolio_Intro_Cards
    {
        public bool auto_invest_shown { get; set; }
        public bool learn_shown { get; set; }
    }

    public class Wallet_Balances
    {
        public string aud { get; set; }
        public string nzd { get; set; }
        public string usd { get; set; }
    }

    public class Portfolio
    {
        public string currency { get; set; }
        public string fund_id { get; set; }
        public string holding_type { get; set; }
        public string shares { get; set; }
        public Stats stats { get; set; }
    }

    public class Stats
    {
        public string shares_bought { get; set; }
        public string shares_sold { get; set; }
        public string shares_transferred_in { get; set; }
        public string shares_transferred_out { get; set; }
    }

    public class User_List
    {
        public string id { get; set; }
        public string preferred_name { get; set; }
        public bool primary { get; set; }
        public string state { get; set; }
    }

}
