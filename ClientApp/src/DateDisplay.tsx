import React, { useEffect, useState } from 'react';
import './style/DateDisplay.css';
import dayjs from './dayjs';
var sprintf = require('sprintf-js').sprintf;



const InteractiveForm = ({user, users, initialUserTimes, initialAllTimes, requestToken, postDateApi}) => {

    type CellCallback = (date: dayjs.Dayjs, e: React.MouseEvent<HTMLElement>) => void;
    type getSlotClass = (cellDate: dayjs.Dayjs) => string;

    const [baseDate, setBaseDate] = useState<dayjs.Dayjs>(dayjs().startOf('day'));
    const [userTimeSlots, setUserTimeSlots] = useState<Map<number, Set<string>>>(ingestUserTimes(initialUserTimes));
    const [allTimeSlots, setAllTimeSlots] = useState<Map<number, Set<string>>>(ingestAllTimes(initialAllTimes));
    const [allUsers, setAllUsers] = useState<Array<string>>(ingestAllUsers(users));

    function ingestUserTimes(initialUserTimes): Map<number, Set<string>> {
        if (!Array.isArray(initialUserTimes)) throw Error("initialUserTimes is not array");
        let injestedTimes = new Map<number, Set<string>>();
        initialUserTimes.forEach((d) => {
            if (!dayjs(d).isValid()) throw Error('date in initialUserTimes invalid: ' + String(d));
            injestedTimes.set(dayjs(d).valueOf(), new Set<string>([user]));
        });
        //console.log("initial user timeslots: " + Array.from(injestedTimes.keys(), (k: number) => dayjs(k).toString()));
        return injestedTimes;
    }

    function ingestAllTimes(initialAllTimes): Map<number, Set<string>> {
        if (initialAllTimes === null) throw Error("initialUserTimes does not exist");
        let ingestedTimes = new Map<number, Set<string>>();
        for (const key in initialAllTimes) {
            var prop = initialAllTimes[key];
            var date = dayjs(key);
            if (!Array.isArray(prop) || !date.isValid()) throw Error('initialAllTimes invalid property: ' + key + ': ' + prop);
            ingestedTimes.set(date.valueOf(), new Set<string>(prop));
        }
        return ingestedTimes;
    }
    function ingestAllUsers(users): Array<string> {
        if (users === null || !Array.isArray(users)) throw Error("users does not exist or is invalid");
        console.log("users: "+ users);
        return users;
    }

    function getAllUsers(timeSlots: Map<number, Set<string>>): Set<string> {
        if (timeSlots === null) throw Error("initialUserTimes does not exist");
        let ingestedUsers = new Set<string>();
        timeSlots.forEach((value: Set<string>, key: number) => {
            value.forEach((u) => ingestedUsers.add(u));
        });
        return ingestedUsers;
    }

    function getOtherUsers(): Set<string> {
        var users = new Set<string>(getAllUsers(allTimeSlots));
        users.delete(user);
        return users;
    }

    function getSlotClassUser(cellDate: dayjs.Dayjs): string {
        var density;
        if (!userTimeSlots.has(cellDate.valueOf())) return "user0";
        else return "user1";
    }

    function getSlotClassSchedule(cellDate: dayjs.Dayjs): string {
        var range = allUsers.length;
        var density;
        if (!allTimeSlots.has(cellDate.valueOf())) return "user0";
        else {
            var slot = allTimeSlots.get(cellDate.valueOf());
            //if (slot.has(user)) {
                if (slot.size === range) return "user3";
                else if (slot.size > 1) return "user2";
                else return "user1";
//            }
//            else {
//                if (slot.size === range) return "other3";
//                else if (slot.size > 1) return "other2";
//0               else return "other1";
//            }
        }
    }

    const PostDatesCallback = async (date: dayjs.Dayjs, e: React.MouseEvent<HTMLElement>) => {
        if (userTimeSlots.has(date.valueOf()))
        {
            userTimeSlots.delete(date.valueOf());
            setUserTimeSlots(new Map(userTimeSlots));
        }
        else setUserTimeSlots(new Map(userTimeSlots.set(date.valueOf(), new Set<string>([user]))));

        if (allTimeSlots.has(date.valueOf())){
            if (allTimeSlots.get(date.valueOf()).has(user)) {
                if (allTimeSlots.get(date.valueOf()).size === 1) allTimeSlots.delete(date.valueOf());
                else allTimeSlots.get(date.valueOf()).delete(user);
            } else {
                allTimeSlots.get(date.valueOf()).add(user);
            }
            setAllTimeSlots(new Map(allTimeSlots));
        }
        else setAllTimeSlots(new Map(allTimeSlots.set(date.valueOf(), new Set<string>([user]))));

        const response = await fetch(postDateApi, {
            method: "POST",
            headers: {
                RequestVerificationToken: requestToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(Array.from(userTimeSlots.keys()))
        });

        if (!response.ok) {
            console.log(`Request Failed: ${response.status}`);
        }
    }

    function genTimeSlotGrid(refSlots: Map<number, Set<string>>, callback: CellCallback, getSlotClass: getSlotClass) {
        return Array.from(Array(24).keys(), h => {
            let hour = h + 1;
            return <tr className='timeRow' key={hour}>
                <td className="timeSlot">
                    <p className="timeRowHeader">
                        {hour > 12 ? sprintf('%d pm', hour - 12) : sprintf('%d am', hour)}
                    </p>
                </td>
                {   Array.from(Array(7).keys(), day => {
                    let cellDate = baseDate.add(day, 'day').hour(hour);
                        return <td
                            className={'timeSlot ' + (refSlots.has(cellDate.valueOf()) ? 'selectedTime ' : 'deselectedTime ')
                                                   + (getSlotClass(cellDate))}
                            key={cellDate.valueOf()}
                            data-date={cellDate.toString()}
                            {... (callback != null ? { onClick : callback.bind(null, cellDate) } : {})}
                        ></td>
                    })
                }
            </tr>
        });
    }

    const dateHeaderCells: Array<JSX.Element> = Array.from(Array(7).keys(), n =>
        <td className='timeSlot' key={n}>
            <p className='timeSlotHeader'> {baseDate.add(n, 'day').format('dddd, MMM D')}</p>
        </td>
    ); 

    const nameList: JSX.Element =
        <div className="dateTableWrapper">
            <h3>Users</h3>
            <div className="userListWrapper">
                {Array.from(allUsers, u =>
                    <div className="nameListItem">
                        {u}
                    </div>)}
            </div>
        </div>


    return (
        <div className="dateDisplayContainer"> 
            <div className="dateTableWrapper">
                <div>
                    <h3>Select your available time slots</h3>
                </div>
                <table className="dateTimeTable">
                    <thead>
                        <tr>
                            <td />
                            {dateHeaderCells}
                        </tr>
                    </thead>
                    <tbody>
                        {genTimeSlotGrid(userTimeSlots, PostDatesCallback, getSlotClassUser)}
                    </tbody>
                </table>
            </div >
            <div className="dateTableWrapper">
                <h3>Schedule</h3>
                <table className="dateTimeTable"> 
                    <thead>
                        <tr>
                            <td />
                            {dateHeaderCells}
                        </tr> 
                    </thead> 
                    <tbody>
                        {genTimeSlotGrid(allTimeSlots, null, getSlotClassSchedule)}
                    </tbody>
                </table>
            </div>
            <div className="nameList">
                {nameList}
            </div>
        </div >
    );
};

export default InteractiveForm;