import React, { useEffect, useState } from 'react';
import './style/DateDisplay.css';
import dayjs from './dayjs';
var sprintf = require('sprintf-js').sprintf;



const InteractiveForm = ({user, users, owner, initialUserTimes, initialAllTimes, requestToken, postDateApi}) => {

    type CellCallback = (date: dayjs.Dayjs, e: React.MouseEvent<HTMLElement>) => void;
    type getSlotClass = (cellDate: dayjs.Dayjs) => string;
    type slotPredicate = (timeSlots: Set<string>) => boolean;

    const [baseDateUser, setBaseDateUser] = useState<dayjs.Dayjs>(dayjs().startOf('day'));
    const [baseDateSchedule, setBaseDateSchedule] = useState<dayjs.Dayjs>(dayjs().startOf('day'));
    const [allTimeSlots, setAllTimeSlots] = useState<Map<number, Set<string>>>(ingestAllTimes(initialAllTimes));
    const [allUsers, setAllUsers] = useState<Array<string>>(ingestAllUsers(users));

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

    function getSlotClassUser(cellDate: dayjs.Dayjs): string {
        if (!allTimeSlots.has(cellDate.valueOf()) || !allTimeSlots.get(cellDate.valueOf()).has(user)) return "user0";
        else return "user1";
    }

    function getSlotClassSchedule(cellDate: dayjs.Dayjs): string {
        var range = allUsers.length;
        if (!allTimeSlots.has(cellDate.valueOf())) return "user0";
        else {
            var slot = allTimeSlots.get(cellDate.valueOf());
            //if (slot.has(user)) {
                if (slot.size === range) return "user3";
                else if (slot.size > 1) return "user2";
                else return "user1";
        }
    }

    function userFilter(users: Set<string>): boolean {
        return users.has(user);
    }

    const PostDatesCallback = async (date: dayjs.Dayjs, e: React.MouseEvent<HTMLElement>) => {
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
            body: JSON.stringify(Array.from(allTimeSlots.keys()).filter(k => allTimeSlots.get(k).has(user)))
        });

        if (!response.ok) {
            console.log(`Request Failed: ${response.status}`);
        }
    }

    function genTimeSlotGrid(refSlots: Map<number, Set<string>>, callback: CellCallback,
        baseDate: dayjs.Dayjs, slotPredicate: slotPredicate, getSlotClass: getSlotClass) {
        return Array.from(Array(24).keys(), h => {
            let hour = h + 1;
            return <tr className='timeRow' key={hour}>
                <td className="timeSlot">
                    <p className="timeRowHeader">
                        {hour > 12 ? sprintf('%d pm', hour - 12) : sprintf('%d am', hour)}
                    </p>
                </td>
                {Array.from(Array(7).keys(), day => {
                    let cellDate = baseDate.add(day, 'day').hour(hour);
                        return <td
                            className={'timeSlot ' + (refSlots.has(cellDate.valueOf()) && (slotPredicate == null || slotPredicate(refSlots.get(cellDate.valueOf()))) ? 'selectedTime ' : 'deselectedTime ')
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

    function genBaseDateControl(baseDate: dayjs.Dayjs, SetBaseDate: React.Dispatch<React.SetStateAction<dayjs.Dayjs>>) {
        return <div className='baseDateControl'>
            <button onClick={() => SetBaseDate(baseDate.add(-1, 'day'))}>&lt;</button>
            <button onClick={() => SetBaseDate(baseDate.add(1, 'day'))}>&gt;</button>
        </div>
    }

    function genDateHeaderCells(baseDate: dayjs.Dayjs) {
        return Array.from(Array(7).keys(), n =>
            <td className='timeSlot' key={n}>
                <p className='timeSlotHeader'> {baseDate.add(n, 'day').format('dddd, MMM D')}</p>
            </td>); 
    }

    const nameList: JSX.Element =
        <div className="dateTableWrapper">
            <h3>Users</h3>
            <div className="userListWrapper">
                {Array.from(allUsers.filter(u => u != owner), u =>
                    <div className="nameListItem">
                        {u}
                    </div>)}
            </div>
        </div>

    const ownerList: JSX.Element =
        <div className="dateTableWrapper">
            <h3>Owner</h3>
            <div className="userListWrapper">
                <div className="nameListItem">
                        {owner}
                </div>
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
                            <td>{genBaseDateControl(baseDateUser, setBaseDateUser)}</td>
                            {genDateHeaderCells(baseDateUser)}
                        </tr>
                    </thead>
                    <tbody>
                        {genTimeSlotGrid(allTimeSlots, PostDatesCallback, baseDateUser, userFilter, getSlotClassUser)}
                    </tbody>
                </table>
            </div >
            <div className="dateTableWrapper">
                <h3>Schedule</h3> 
                <table className="dateTimeTable"> 
                    <thead>
                        <tr>
                            <td>{genBaseDateControl(baseDateSchedule, setBaseDateSchedule)}</td>
                            {genDateHeaderCells(baseDateSchedule)}
                        </tr> 
                    </thead> 
                    <tbody>
                        {genTimeSlotGrid(allTimeSlots,null, baseDateSchedule, null, getSlotClassSchedule)}
                    </tbody>
                </table>
            </div>
            <div className="ListContainer">
                <div className="nameList">
                    {ownerList}
                </div>
                <div className="nameList">
                    {nameList}
                </div>
            </div>

        </div >
    );
};

export default InteractiveForm;