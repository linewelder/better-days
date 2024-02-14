﻿/**
 * The hour at which the next day starts.
 */
const perceivedDayBoundary = 6;

/**
 * If it is after midnight, the note is most likely to be
 * about the day that has just ended.
 */
function getPerceivedToday() {
    const today = new Date();
    const stillNotSleeping = today.getHours() < perceivedDayBoundary;
    if (stillNotSleeping) {
        today.setDate(today.getDate() - 1);
    }

    return today;
}

const daysOfWeek = [
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
    "Sunday"
];

const months = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
];

/**
 * Formats the date in the human readable format ("dddd, d mmm yyyy" in C#).
 */
function formatDate(date) {
    const dayOfWeek = daysOfWeek[date.getDay()];
    const day = date.getDate();
    const month = months[date.getMonth()];
    const year = date.getFullYear();
    return `${dayOfWeek}, ${day} ${month} ${year}`;
}
